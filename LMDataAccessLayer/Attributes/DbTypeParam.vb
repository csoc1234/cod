<AttributeUsage(AttributeTargets.All)>
Public Class DbTypeParam : Inherits Attribute
    Public MyDbType As DbType
    Public Sub New(DbTyped As DbType)
        MyDbType = DbTyped
    End Sub
End Class
