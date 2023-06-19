
Imports LMDataAccessLayer


Public Class ConfigValues

#Region "Atributos (Campos)"

        Public Property IdConfig As Integer
        Public Property ConfigKeyName As String
        Public Property ConfigKeyValue As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal configKeyName As String)
            MyBase.New()
            _ConfigKeyName = configKeyName
            CargarInformacion()
        End Sub

#End Region


#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _ConfigKeyName IsNot Nothing AndAlso _ConfigKeyName.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@configKeyName", SqlDbType.VarChar, 50).Value = _ConfigKeyName
                        .ejecutarReader("ObtenerInfoConfigValues", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _ConfigKeyValue = .Reader("configKeyValue").ToString
                                Integer.TryParse(.Reader("idConfig").ToString, _IdConfig)
                            End If
                            .Reader.Close()
                        End If

                        If Not .Reader.IsClosed Then .Reader.Close()
                    End With
                Catch ex As Exception
                    Throw New ArgumentNullException(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region


    End Class

