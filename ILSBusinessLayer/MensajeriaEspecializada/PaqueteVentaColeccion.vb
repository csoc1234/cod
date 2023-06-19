﻿Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada

Public Class PaqueteVentaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idPaquete As Integer
    Private _nombrePaquete As String
    Private _activo As Nullable(Of Boolean)

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal activo As Boolean)
        MyBase.New()
        _activo = activo
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As PaqueteVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As PaqueteVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdPlan As Short
        Get
            Return _idPaquete
        End Get
        Set(value As Short)
            _idPaquete = value
        End Set
    End Property

    Public Property NombrePaquete As String
        Get
            Return _nombrePaquete
        End Get
        Set(value As String)
            _nombrePaquete = value
        End Set
    End Property

    Public Property Activo As Nullable(Of Boolean)
        Get
            Return _activo
        End Get
        Set(value As Nullable(Of Boolean))
            _activo = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(PaqueteVenta)
        Dim pInfo As PropertyInfo

        For Each pInfo In miObj.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                With dtAux
                    .Columns.Add(pInfo.Name, GetType(Boolean))
                End With
            End If
        Next
        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PaqueteVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As PaqueteVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As PaqueteVentaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As PaqueteVenta)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idPaquete As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), PaqueteVenta)
                If .IdPaquete = idPaquete Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Try
            Dim drAux As DataRow
            Dim miDetalle As PaqueteVenta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), PaqueteVenta)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(PaqueteVenta).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Clear()

                    If _idPaquete > 0 Then .SqlParametros.Add("@idPaquete", SqlDbType.VarChar).Value = _idPaquete
                    If Not String.IsNullOrEmpty(_nombrePaquete) Then .SqlParametros.Add("@nombrePaquete", SqlDbType.VarChar).Value = _nombrePaquete
                    .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtienePaquetesVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As PaqueteVenta
                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New PaqueteVenta
                                elDetalle.CargarResultadoConsulta(.Reader)
                                _cargado = True
                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End With
        End Using
    End Sub

#End Region

End Class
