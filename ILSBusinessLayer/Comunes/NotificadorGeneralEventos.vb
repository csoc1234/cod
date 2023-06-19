Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Recibos
Imports System.Collections.Generic

Namespace Comunes

    Public Class NotificadorGeneralEventos

#Region "Atributos"

        Private _inicioMensaje As String
        Private _finMensaje As String
        Private _titulo As String
        Private _asunto As String
        Private _mensaje As String
        Private _firmaMensaje As String
        Private _mailRespuesta As String
        Private _usuarioRespuesta As String
        Private _tipoNotificacion As Comunes.AsuntoNotificacion.Tipo
        Private _dtImagenes As DataTable
        Private _dtDatos As DataTable
        Private _adjuntosURL As New ArrayList
        Private _destinatariosPrincipal As String
        Private _destinatariosCopia As String

#End Region

#Region "Propiedades"

        Public Property InicioMensaje As String
            Get
                Return _inicioMensaje
            End Get
            Set(value As String)
                _inicioMensaje = value
            End Set
        End Property

        Public Property FinMensaje As String
            Get
                Return _finMensaje
            End Get
            Set(value As String)
                _finMensaje = value
            End Set
        End Property

        Public Property Titulo As String
            Get
                Return _titulo
            End Get
            Set(value As String)
                _titulo = value
            End Set
        End Property

        Public Property Asunto As String
            Get
                Return _asunto
            End Get
            Set(value As String)
                _asunto = value
            End Set
        End Property

        Public Property MailRespuesta As String
            Get
                Return _mailRespuesta
            End Get
            Set(value As String)
                _mailRespuesta = value
            End Set
        End Property

        Public Property UsuarioRespuesta As String
            Get
                Return _usuarioRespuesta
            End Get
            Set(value As String)
                _usuarioRespuesta = value
            End Set
        End Property

        Public Property FirmaMensaje As String
            Get
                Return _firmaMensaje
            End Get
            Set(value As String)
                _firmaMensaje = value
            End Set
        End Property

        Public Property TipoNotificacion As Comunes.AsuntoNotificacion.Tipo
            Get
                Return _tipoNotificacion
            End Get
            Set(value As Comunes.AsuntoNotificacion.Tipo)
                _tipoNotificacion = value
            End Set
        End Property

        Public Property dtImagenes() As DataTable
            Get
                Return _dtImagenes
            End Get
            Set(value As DataTable)
                _dtImagenes = value
            End Set
        End Property

        Public Property dtDatos() As DataTable
            Get
                Return _dtDatos
            End Get
            Set(value As DataTable)
                _dtDatos = value
            End Set
        End Property

        Public Property Mensaje() As String
            Get
                Return _mensaje
            End Get
            Set(value As String)
                _mensaje = value
            End Set
        End Property

        Public Property AdjuntosURL() As ArrayList
            Get
                Return _adjuntosURL
            End Get
            Set(ByVal value As ArrayList)
                _adjuntosURL = value
            End Set
        End Property

        Public Property DestinatarioPrincipal As String
            Get
                Return _destinatariosPrincipal
            End Get
            Set(value As String)
                _destinatariosPrincipal = value
            End Set
        End Property

        Public Property DestinatarioCopia As String
            Get
                Return _destinatariosCopia
            End Get
            Set(value As String)
                _destinatariosCopia = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Function NotificacionEvento(Optional ByVal mensaje As String = "", Optional mensajeDetalle As String = "", Optional idBodega As Integer = 0, _
                                           Optional ByVal usuarioUnicoNotificacion As String = "") As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder

            Try
                With sbContenido
                    .Append(_inicioMensaje)
                    If mensaje <> "" Then
                        .Append("<br/> " & mensaje)
                    End If
                    .Append("<br/> " & _finMensaje)
                    If mensajeDetalle <> "" Then
                        .Append("<br/> " & mensajeDetalle)
                    End If
                End With

                With Notificacion
                    CargarDestinatarios(DestinosPP, DestinosCC, idBodega, usuarioUnicoNotificacion)
                    .Titulo = _titulo
                    .Asunto = _asunto
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = _firmaMensaje
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    If _adjuntosURL IsNot Nothing Then
                        .RutaAttachment = _adjuntosURL
                    End If
                    If Not String.IsNullOrEmpty(_mailRespuesta) Or Not String.IsNullOrEmpty(_usuarioRespuesta) Then _
                        .EstablecerCuentaRespuesta(_mailRespuesta, _usuarioRespuesta)
                    If Not .EnviarMail() Then
                        respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                    End If
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function


        Public Function NotificacionEventoAdjunto() As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder
            Dim vistaAlterna As AlternateView = Nothing
            Try
                With sbContenido
                    .Append(_mensaje)
                End With

                With Notificacion
                    If _destinatariosPrincipal.Trim.Length <> 0 Or _destinatariosCopia.Trim.Length <> 0 Then
                        If _destinatariosPrincipal.Trim.Length > 0 Then DestinosPP.Add(_destinatariosPrincipal)
                        If _destinatariosCopia.Trim.Length > 0 Then DestinosCC.Add(_destinatariosCopia)
                    Else
                        CargarDestinatariosNotificacion(_tipoNotificacion, DestinosPP, DestinosCC)
                    End If
                    .Titulo = _titulo
                    .Asunto = _asunto
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    '.VistaAlternativa.Add(vistaAlterna)
                    .RutaAttachment = _adjuntosURL
                    If Not .EnviarMail() Then
                        respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                    End If
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub CargarDestinatarios(ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection, Optional ByVal idBodega As Integer = 0, _
                                        Optional ByVal usuarioUnicoNotificacion As String = "")
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP As String = String.Empty
            Dim strDestinoCC As String = String.Empty

            If usuarioUnicoNotificacion = "" Then
                filtro.IdAsuntoNotificacion = _tipoNotificacion
                If idBodega <> 0 Then
                    filtro.IdBodega = idBodega
                End If
                filtro.Separador = ","
                Try
                    dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                    For Each fila As DataRow In dtDestinos.Rows
                        strDestinoPP += fila.Item("destinoPara")
                        strDestinoCC += fila.Item("destinoCopia")
                    Next

                    destinoPP.Add(strDestinoPP)
                    destinoCC.Add(strDestinoCC)

                Catch ex As Exception
                Finally
                    If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
                End Try
            Else
                strDestinoPP += (usuarioUnicoNotificacion)
                destinoPP.Add(strDestinoPP)
            End If

        End Sub


        Private Sub CargarDestinatariosNotificacion(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection)
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP As String = String.Empty
            Dim strDestinoCC As String = String.Empty

            filtro.IdAsuntoNotificacion = tipo
            filtro.Separador = ","
            Try
                dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                For Each fila As DataRow In dtDestinos.Rows
                    strDestinoPP += fila.Item("destinoPara")
                    strDestinoCC += fila.Item("destinoCopia")
                Next

                If (strDestinoPP <> String.Empty And strDestinoPP <> "") Then
                    destinoPP.Add(strDestinoPP)
                End If
                If (strDestinoCC <> String.Empty And strDestinoCC <> "") Then
                    destinoCC.Add(strDestinoCC)
                End If



            Catch ex As Exception
            Finally
                If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
            End Try
        End Sub

#End Region

    End Class

End Namespace