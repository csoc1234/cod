Public Class InfoSesion

    Public Property Username As String
    Public Property Password As String

    Public Sub New()
        Dim raizRutaCarpeta As New Comunes.ConfigValues("USUARIOCLAVEPRESENCE")
        Dim utenticacionCorre() As String = raizRutaCarpeta.ConfigKeyValue.Split("|")

        Username = utenticacionCorre(0)
        Password = utenticacionCorre(1)
    End Sub

End Class
