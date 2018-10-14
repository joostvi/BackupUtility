Public Class Commands

    Private ReadOnly _showHelp As Boolean
    Private ReadOnly _source As String
    Private ReadOnly _target As String
    Private ReadOnly _updatedOnly As Boolean
    Private ReadOnly _requestConfirm As Boolean
    Private ReadOnly _subFoldersAlso As Boolean
    Private ReadOnly _skipCopyErrors As Boolean
    Private ReadOnly _pauseWhenDone As Boolean
    Private ReadOnly _readCheckFirst As Boolean

    Private ReadOnly _exclusiveExt As String()

    Private Class CommandStringComparer
        Implements IEqualityComparer(Of String)

        Public Function IsEqual(ByVal x As String, ByVal y As String) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of String).Equals
            Dim equal As Boolean = False

            If x Is Nothing AndAlso y Is Nothing Then
                equal = True
            ElseIf x Is Nothing Then
                equal = False
            ElseIf y Is Nothing Then
                equal = False
            Else
                equal = x.ToUpper = y.ToUpper
            End If
            Return equal
        End Function

        Public Function GetHashCode1(ByVal obj As String) As Integer Implements System.Collections.Generic.IEqualityComparer(Of String).GetHashCode
            If obj Is Nothing Then Return "".GetHashCode
            Return obj.GetHashCode
        End Function
    End Class

    Private Function FormatTarget(ByVal thisTarget As String) As String
        thisTarget = PathFormatter.FormatPath(thisTarget)
        If Not thisTarget.EndsWith("/") _
            AndAlso Not thisTarget.EndsWith("\") Then
            thisTarget &= "\"
        End If
        Return thisTarget
    End Function

    Public Sub New(ByVal args() As String)
        _showHelp = False
        _source = ""
        _updatedOnly = False
        _requestConfirm = True
        _subFoldersAlso = False
        _skipCopyErrors = False
        _pauseWhenDone = False
        ReDim _exclusiveExt(0)
        'If not arguments supplied or help is requested set showHelp to true and stop processing.
        If args Is Nothing OrElse args.Length < 2 OrElse args.Contains("/?") Then
            _showHelp = True
        Else
            'Check other parms 
            '0 should be source
            Console.WriteLine("args(0)=" & args(0))
            _source = PathFormatter.FormatPath(args(0))
            Console.WriteLine("source=" & _source)
            '1 should be targed
            _target = FormatTarget(args(1))
            _updatedOnly = args.Contains("/d", New CommandStringComparer)
            _requestConfirm = Not args.Contains("/y", New CommandStringComparer)
            _subFoldersAlso = args.Contains("/s", New CommandStringComparer)
            _skipCopyErrors = args.Contains("/x", New CommandStringComparer)
            _pauseWhenDone = args.Contains("/p", New CommandStringComparer)
            _readCheckFirst = args.Contains("/r", New CommandStringComparer)

            For Each aCmd As String In args
                If aCmd.ToLower.StartsWith("/exclusiveext:") Then
                    _exclusiveExt = Split(Split(aCmd, ":")(1), "+")
                    Exit For
                End If
            Next
        End If
    End Sub

    Public Shared Function Help() As String
        Dim aStr As String

        'TODO: Make language depending.

        aStr = "Description of ZCopy version " & My.Application.Info.Version.ToString()
        aStr &= vbCrLf & "===================================================="
        aStr &= vbCrLf & "ZCopy is an utility to copy files and directories. "
        aStr &= vbCrLf & "This tool is created as a helper because xcopy did not work for me!"
        aStr &= vbCrLf & "As I wanted xcopy only to copy new or updated files and did failed for my Nas."
        aStr &= vbCrLf & "Also I wanted to do some practice in another style of programma as I'm processional used."
        aStr &= vbCrLf & vbCrLf & "Parameters"
        aStr &= vbCrLf & "===================================================="
        aStr &= vbCrLf & "This utility accepts the next parameters:"
        aStr &= vbCrLf & "The source which to copy from this is mandetory and should be the first parameter."
        aStr &= vbCrLf & "The target which to copy to this is mandetory and should be second parameter."
        aStr &= vbCrLf & "/d Copy only new or updated files."
        aStr &= vbCrLf & "/y do not request for confirmation."
        aStr &= vbCrLf & "/s copy subfolders also."
        aStr &= vbCrLf & "/x skip errors. Program will continue with next directory or folder if an error occurs."
        aStr &= vbCrLf & "/exclusiveExt: list separated by + of file extensions which should be skipped when performing the copy action."
        aStr &= vbCrLf & "/? show this info file. When this parameter is given all others will be ignored."
        aStr &= vbCrLf & "/p Pause when copy is ready."
        aStr &= vbCrLf & "/r First check if we can read the file."
        aStr &= vbCrLf & vbCrLf & "Switches may be in any order as long as they are not used as first or second parameter."
        aStr &= vbCrLf & vbCrLf & "Example:"
        aStr &= vbCrLf & vbCrLf & "zcopy c:\tmp\ d:\tmp /d /y /s /x /exclusiveExt:bak+csv"
        Return aStr
    End Function

    Private Sub New(ByVal showHelp As Boolean, ByVal source As String, ByVal target As String, _
        ByVal updatedOnly As Boolean, ByVal requestConfirm As Boolean, ByVal subFoldersAlso As Boolean, _
        ByVal skipCopyErrors As Boolean, ByVal exclusiveExt() As String, ByVal pauseWhenDone As Boolean, _
        ByVal doReadCheckFirst As Boolean)
        _showHelp = showHelp
        _source = source
        _target = target
        _updatedOnly = updatedOnly
        _requestConfirm = requestConfirm
        _subFoldersAlso = subFoldersAlso
        _skipCopyErrors = skipCopyErrors
        _exclusiveExt = exclusiveExt
        _pauseWhenDone = pauseWhenDone
        _readCheckFirst = doReadCheckFirst
    End Sub

    Public ReadOnly Property ShowHelp() As Boolean
        Get
            Return _showHelp
        End Get
    End Property

    Public ReadOnly Property Source() As String
        Get
            Return _source
        End Get
    End Property

    Public ReadOnly Property Target() As String
        Get
            Return _target
        End Get
    End Property

    Public ReadOnly Property UpdatedOnly() As Boolean
        Get
            Return _updatedOnly
        End Get
    End Property

    Public ReadOnly Property RequestConfirm() As Boolean
        Get
            Return _requestConfirm
        End Get
    End Property

    Public ReadOnly Property SubFoldersAlso() As Boolean
        Get
            Return _subFoldersAlso
        End Get
    End Property

    Public ReadOnly Property SkipCopyErrors() As Boolean
        Get
            Return _skipCopyErrors
        End Get
    End Property

    Public ReadOnly Property ExclusiveExt() As String()
        Get
            Return _exclusiveExt
        End Get
    End Property

    Public ReadOnly Property PauseWhenDone() As Boolean
        Get
            Return _pauseWhenDone
        End Get
    End Property

    Public ReadOnly Property ReadCheckFirst() As Boolean
        Get
            Return _readCheckFirst
        End Get
    End Property

    Public Function Clone() As Commands
        Return New Commands(Me.ShowHelp, Me.Source, Me.Target, Me.UpdatedOnly, Me.RequestConfirm, Me.SubFoldersAlso, _
                            Me.SkipCopyErrors, Me.ExclusiveExt, Me.PauseWhenDone, Me.ReadCheckFirst)
    End Function

End Class

