Namespace My
    ' Per MyApplication sono disponibili gli eventi seguenti:
    ' Startup: generato all'avvio dell'applicazione, prima della creazione del form di avvio.
    ' Shutdown: generato dopo la chiusura di tutti i form dell'applicazione. Questo evento non viene generato se l'applicazione termina in modo anomalo.
    ' UnhandledException: generato se nell'applicazione si verifica un'eccezione non gestita.
    ' StartupNextInstance: generato all'avvio di un'applicazione a istanza singola se l'applicazione è già attiva. 
    ' NetworkAvailabilityChanged: generato quando la connessione di rete viene connessa o disconnessa.
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            e.ExitApplication = Not (MsgBox($"Ops Errore ...{vbCrLf}{vbCrLf}{e.Exception.Message}{vbCrLf}Chiudere l'applicazione ?", MsgBoxStyle.YesNo) = MsgBoxResult.No)

        End Sub

    End Class
End Namespace
