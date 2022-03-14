<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelCommand = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonLoadGCode = New System.Windows.Forms.Button()
        Me.ButtonSaveOpenScad = New System.Windows.Forms.Button()
        Me.TextBoxGCode = New System.Windows.Forms.TextBox()
        Me.TextBoxOpenScad = New System.Windows.Forms.TextBox()
        Me.ButtonTest = New System.Windows.Forms.Button()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.TableLayoutPanelCommand.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 1
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanelCommand, 0, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxGCode, 0, 1)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxOpenScad, 0, 2)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 3
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(800, 450)
        Me.TableLayoutPanelMain.TabIndex = 0
        '
        'TableLayoutPanelCommand
        '
        Me.TableLayoutPanelCommand.ColumnCount = 3
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonLoadGCode, 0, 0)
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonSaveOpenScad, 1, 0)
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonTest, 2, 0)
        Me.TableLayoutPanelCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelCommand.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelCommand.Name = "TableLayoutPanelCommand"
        Me.TableLayoutPanelCommand.RowCount = 1
        Me.TableLayoutPanelCommand.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelCommand.Size = New System.Drawing.Size(794, 39)
        Me.TableLayoutPanelCommand.TabIndex = 0
        '
        'ButtonLoadGCode
        '
        Me.ButtonLoadGCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonLoadGCode.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLoadGCode.Name = "ButtonLoadGCode"
        Me.ButtonLoadGCode.Size = New System.Drawing.Size(258, 33)
        Me.ButtonLoadGCode.TabIndex = 0
        Me.ButtonLoadGCode.Text = "LOAD G-CODE"
        Me.ButtonLoadGCode.UseVisualStyleBackColor = True
        '
        'ButtonSaveOpenScad
        '
        Me.ButtonSaveOpenScad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSaveOpenScad.Location = New System.Drawing.Point(267, 3)
        Me.ButtonSaveOpenScad.Name = "ButtonSaveOpenScad"
        Me.ButtonSaveOpenScad.Size = New System.Drawing.Size(258, 33)
        Me.ButtonSaveOpenScad.TabIndex = 1
        Me.ButtonSaveOpenScad.Text = "SAVE OPEN SCAD"
        Me.ButtonSaveOpenScad.UseVisualStyleBackColor = True
        '
        'TextBoxGCode
        '
        Me.TextBoxGCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxGCode.Location = New System.Drawing.Point(3, 48)
        Me.TextBoxGCode.Multiline = True
        Me.TextBoxGCode.Name = "TextBoxGCode"
        Me.TextBoxGCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxGCode.Size = New System.Drawing.Size(794, 196)
        Me.TextBoxGCode.TabIndex = 1
        '
        'TextBoxOpenScad
        '
        Me.TextBoxOpenScad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxOpenScad.Location = New System.Drawing.Point(3, 250)
        Me.TextBoxOpenScad.Multiline = True
        Me.TextBoxOpenScad.Name = "TextBoxOpenScad"
        Me.TextBoxOpenScad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxOpenScad.Size = New System.Drawing.Size(794, 197)
        Me.TextBoxOpenScad.TabIndex = 2
        '
        'ButtonTest
        '
        Me.ButtonTest.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonTest.Location = New System.Drawing.Point(531, 3)
        Me.ButtonTest.Name = "ButtonTest"
        Me.ButtonTest.Size = New System.Drawing.Size(260, 33)
        Me.ButtonTest.TabIndex = 2
        Me.ButtonTest.Text = "TEST"
        Me.ButtonTest.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "G-Code Tools"
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.TableLayoutPanelMain.PerformLayout()
        Me.TableLayoutPanelCommand.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCommand As TableLayoutPanel
    Friend WithEvents ButtonLoadGCode As Button
    Friend WithEvents ButtonSaveOpenScad As Button
    Friend WithEvents TextBoxGCode As TextBox
    Friend WithEvents TextBoxOpenScad As TextBox
    Friend WithEvents ButtonTest As Button
End Class
