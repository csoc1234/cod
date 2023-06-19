Public Class EncryptionData
    Private edArrByte() As Byte
    Private edMaxBytes As Integer
    Private edMinBytes As Integer
    Private edStepBytes As Integer

    ''' <summary>
    ''' Determina la codificación de texto por defecto para todas las instancias de EncryptionData
    ''' </summary>
    Public Shared DefaultEncoding As Text.Encoding = System.Text.Encoding.Default

    ''' <summary>
    ''' Determina la codificación de texto por defecto de esta instancia de EncryptionData
    ''' </summary>
    Public Encoding As Text.Encoding = DefaultEncoding

    ''' <summary>
    ''' Crear una nueva instancia
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Crear una nueva instanci con el array de Bytes especificado
    ''' </summary>
    Public Sub New(ByVal arrByte As Byte())
        Me.edArrByte = arrByte
    End Sub


    ''' <summary>
    ''' Retorna true si no hay datos presentes
    ''' </summary>
    Public ReadOnly Property IsEmpty() As Boolean
        Get
            If Me.edArrByte Is Nothing OrElse Me.edArrByte.Length = 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Intervalos de avance permitidos en bytes para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property StepBytes() As Integer
        Get
            Return Me.edStepBytes
        End Get
        Set(ByVal Value As Integer)
            Me.edStepBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' Intervalos de avance permitidos en bits para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property StepBits() As Integer
        Get
            Return Me.edStepBytes * 8
        End Get
        Set(ByVal Value As Integer)
            Me.edStepBytes = Value \ 8
        End Set
    End Property

    ''' <summary>
    ''' Mínimo número de bytes permitidos para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property MinBytes() As Integer
        Get
            Return Me.edMinBytes
        End Get
        Set(ByVal Value As Integer)
            Me.edMinBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' Mínimo número de bits permitidos para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property MinBits() As Integer
        Get
            Return Me.edMinBytes * 8
        End Get
        Set(ByVal Value As Integer)
            Me.edMinBytes = Value \ 8
        End Set
    End Property

    ''' <summary>
    ''' Maximo número de bytes permitidos para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property MaxBytes() As Integer
        Get
            Return Me.edMaxBytes
        End Get
        Set(ByVal Value As Integer)
            Me.edMaxBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' Máximo número de bits permitidos para los datos de la clase. Si el valor es 0, no existe limite
    ''' </summary>
    Public Property MaxBits() As Integer
        Get
            Return Me.edMaxBytes * 8
        End Get
        Set(ByVal Value As Integer)
            Me.edMaxBytes = Value \ 8
        End Set
    End Property

End Class
