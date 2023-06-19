Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class MatrizTipoClientePlanRecaudo
#Region "Atributos (Campos)"

        Private _idTipoCliente As Integer
        Private _tipoCliente As String
        Private _idPlan As Integer
        Private _nombrePlan As String
        Private _requiereCFM As Boolean
        Private _numeroCuotas As Integer
        Private _cargoFijoMensual As Double

        Private _registrado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoCliente As Integer, ByVal idPlan As Integer)
            Me.New()
            _idPlan = idPlan
            _idTipoCliente = idTipoCliente
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdTipoCliente As Integer
            Get
                Return _idTipoCliente
            End Get
            Set(value As Integer)
                _idTipoCliente = value
            End Set
        End Property

        Public Property TipoCliente As String
            Get
                Return _tipoCliente
            End Get
            Protected Friend Set(value As String)
                _tipoCliente = value
            End Set
        End Property

        Public Property IdPlan As Integer
            Get
                Return _idPlan
            End Get
            Set(value As Integer)
                _idPlan = value
            End Set
        End Property

        Public Property NombrePlan As String
            Get
                Return _nombrePlan
            End Get
            Protected Friend Set(value As String)
                _nombrePlan = value
            End Set
        End Property

        Public Property RequiereCFM As Boolean
            Get
                Return _requiereCFM
            End Get
            Set(value As Boolean)
                _requiereCFM = value
            End Set
        End Property

        Public Property NumeroCuotas As Integer
            Get
                Return _numeroCuotas
            End Get
            Protected Friend Set(value As Integer)
                _numeroCuotas = value
            End Set
        End Property

        Public Property CargoFijoMensual As Double
            Get
                Return _cargoFijoMensual
            End Get
            Protected Friend Set(value As Double)
                _cargoFijoMensual = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idTipoCliente > 0 OrElse _idPlan > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            If _idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.Int).Value = _idTipoCliente
                            If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan

                            .ejecutarReader("ObtenerDetalleMatrizTipoClientePlanRecaudo", CommandType.StoredProcedure)
                            If .Reader IsNot Nothing Then
                                If .Reader.Read() Then CargarResultadoConsulta(.Reader)
                                If Not .Reader.IsClosed Then .Reader.Close()
                            End If
                        End With
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idTipoCliente").ToString, _idTipoCliente)
                    _tipoCliente = reader("tipo_cliente").ToString
                    Integer.TryParse(reader("idPlan").ToString, _idPlan)
                    _nombrePlan = reader("nombrePlan").ToString
                    _requiereCFM = CBool(reader("requiereCFM").ToString)
                    Integer.TryParse(reader("numeroCuotas").ToString, _numeroCuotas)
                    Double.TryParse(reader("cargoFijoMensual").ToString, _cargoFijoMensual)

                    _registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"


#End Region

    End Class

End Namespace