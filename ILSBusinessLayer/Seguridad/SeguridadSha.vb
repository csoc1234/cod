Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class SeguridadSha
    Public Shared Function EncryptPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hashedBytes As Byte() = sha256.ComputeHash(bytes)
            Dim stringBuilder As New StringBuilder()
            For Each b As Byte In hashedBytes
                stringBuilder.Append(b.ToString("x2"))
            Next
            Return stringBuilder.ToString()
        End Using
    End Function
    Private Shared Function GenerateRandomBytes(length As Integer) As Byte()
        Dim randomBytes(length - 1) As Byte
        Using rngCsp As New RNGCryptoServiceProvider()
            rngCsp.GetBytes(randomBytes)
        End Using
        Return randomBytes
    End Function
    Public Shared Function EncryptData(data As String, key As String, iv As String) As String
        Dim encryptedData As Byte()
        Try

            Using aesAlg As Aes = Aes.Create()
                aesAlg.Key = Encoding.UTF8.GetBytes(key)
                aesAlg.IV = Encoding.UTF8.GetBytes(iv)
                Using encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                    Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)
                    encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length)
                End Using
            End Using
        Catch ex As Exception

        End Try

        Return Convert.ToBase64String(encryptedData)
    End Function
    Public Shared Function DecryptData(encryptedData As String, key As String, iv As String) As String
        Dim decryptedData As String
        Using aesAlg As Aes = Aes.Create()
            aesAlg.Key = Encoding.UTF8.GetBytes(key)
            aesAlg.IV = Encoding.UTF8.GetBytes(iv)
            Using decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                Using msDecrypt As New MemoryStream(Convert.FromBase64String(encryptedData))
                    Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                        Using srDecrypt As New StreamReader(csDecrypt)
                            decryptedData = srDecrypt.ReadToEnd()
                        End Using
                    End Using
                End Using
            End Using
        End Using
        Return decryptedData
    End Function
End Class
