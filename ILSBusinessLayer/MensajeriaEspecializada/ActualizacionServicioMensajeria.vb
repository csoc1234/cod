Imports LMDataAccessLayer

Public Class ActualizacionServicioMensajeria

#Region "Metodos"

    Public Function ActualizaServicioMensajeria(ByVal idServicio As Integer, idResponsableMensajeria As Integer,
                                                      idZona As Integer, idUsuarioLog As Integer, cedulaMotorizado As String,
                                                      nombresApellidos As String,
                                                      idTipoServicio As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resAM As New NotusExpressBancolombiaService.ResultadoProceso
        Dim enumeracion As New Enumerados.TipoServicio
        Try
            If idZona <> 0 Then

                With dbManager
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.Int).Value = idServicio
                        If idResponsableMensajeria <> 0 Then
                            .Add("@idResponsableEntrega", SqlDbType.Int).Value = idResponsableMensajeria
                        End If
                        .Add("@idZona", SqlDbType.Int).Value = idZona
                        .Add("@idUsuarioLog", SqlDbType.VarChar).Value = idUsuarioLog
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizaServicioMensajeria", CommandType.StoredProcedure)
                    .ConfirmarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Motorizado asignado correctamente")
                    If idTipoServicio = Enumerados.TipoServicio.ServiciosFinancierosBancolombia Then
                        Dim notusEBS As New NotusExpressBancolombiaService.NotusExpressBancolombiaService
                        resAM = notusEBS.AsignarMotorizado(idServicio, cedulaMotorizado, nombresApellidos)
                        If resAM.Valor = 1 Then
                            resultado.EstablecerMensajeYValor(resAM.Valor, "Motorizado asignado correctamente")
                        Else
                            resultado.EstablecerMensajeYValor("-501", "Motorizado asignado correctamente, pero no se pudo lograr conexion con el servicio web de notus express: " & resAM.Mensaje)
                        End If
                    End If

                End With
            Else
                resultado.EstablecerMensajeYValor(0, "Debe seleccionar una Zona para el servicio.")
            End If

        Catch ex As Exception
            dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor("-502", "Error al guardar la asignación de motorizado: " & ex.Message)
        End Try

        Return resultado
    End Function

#End Region
End Class
