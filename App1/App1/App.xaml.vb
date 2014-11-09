' “空白应用程序”模板在 http://go.microsoft.com/fwlink/?LinkID=391641 上有介绍

''' <summary>
''' 提供特定于应用程序的行为，以补充默认的应用程序类。
''' </summary>
NotInheritable Class App
    Inherits Application

    Private _transitions As TransitionCollection

    ''' <summary>
    ''' 初始化单一实例应用程序对象。这是创作的代码的第一行
    ''' 逻辑上等同于 main() 或 WinMain()。
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' 在最终用户正常启动应用程序时调用。将在启动应用程序
    ''' 当启动应用程序以执行打开特定的文件或显示搜索结果等操作时
    ''' 将使用其他入口点。
    ''' </summary>
    ''' <param name="e">有关启动请求和过程的详细信息。</param>
    Protected Overrides Sub OnLaunched(e As LaunchActivatedEventArgs)
#If DEBUG Then
        If System.Diagnostics.Debugger.IsAttached Then
            DebugSettings.EnableFrameRateCounter = True
        End If
#End If

        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' 不要在窗口已包含内容时重复应用程序初始化，
        ' 只需确保窗口处于活动状态
        If rootFrame Is Nothing Then
            ' 创建要充当导航上下文的框架，并导航到第一页
            rootFrame = New Frame()

            ' TODO: 将此值更改为适合您的应用程序的缓存大小
            rootFrame.CacheSize = 1

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO:  从之前挂起的应用程序加载状态
            End If

            ' 将框架放在当前窗口中
            Window.Current.Content = rootFrame
        End If

        If rootFrame.Content Is Nothing Then
            ' 删除用于启动的旋转门导航。
            If rootFrame.ContentTransitions IsNot Nothing Then
                _transitions = New TransitionCollection()
                For Each transition As Transition In rootFrame.ContentTransitions
                    _transitions.Add(transition)
                Next
            End If

            rootFrame.ContentTransitions = Nothing
            AddHandler rootFrame.Navigated, AddressOf RootFrame_FirstNavigated

            ' 当未还原导航堆栈时，导航到第一页，
            ' 并通过将所需信息作为导航参数传入来配置
            ' 参数
            If Not rootFrame.Navigate(GetType(MainPage), e.Arguments) Then
                Throw New Exception("Failed to create initial page")
            End If
        End If

        ' 确保当前窗口处于活动状态
        Window.Current.Activate()
    End Sub

    ''' <summary>
    ''' 启动应用程序后还原内容转换。
    ''' </summary>
    Private Sub RootFrame_FirstNavigated(sender As Object, e As NavigationEventArgs)
        Dim newTransitions As TransitionCollection
        If _transitions Is Nothing Then
            newTransitions = New TransitionCollection()
            newTransitions.Add(New NavigationThemeTransition())
        Else
            newTransitions = _transitions
        End If

        Dim rootFrame As Frame = DirectCast(sender, Frame)
        rootFrame.ContentTransitions = newTransitions
        RemoveHandler rootFrame.Navigated, AddressOf RootFrame_FirstNavigated
    End Sub

    ''' <summary>
    ''' 在将要挂起应用程序执行时调用。将保存应用程序状态
    ''' 将被终止还是恢复的情况下保存应用程序状态，
    ''' 并让内存内容保持不变。
    ''' </summary>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()

        ' TODO:  保存应用程序状态并停止任何后台活动
        deferral.Complete()
    End Sub

End Class
