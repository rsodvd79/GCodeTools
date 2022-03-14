﻿Imports System.ComponentModel
Imports System.Globalization
Imports System.Text

Public Class frmMain
    Dim Ugello As Single = 0.4
    Dim dblLayer As Single = 0.2
    Dim Segmenti As List(Of clsSegmento)
    Dim Processi As List(Of clsProcesso)
    Dim ConteggioSegmentiScadStl As Integer = 0
    Dim StlOutput As String = String.Empty
    Dim GCodeInput As String = String.Empty
    Dim TempFolder As String = String.Empty
    Dim GruppoSegmenti As Integer = 1
    Dim GruppoProcessi As Integer = 1
    Dim ConservaSTL As Boolean = False
    Dim ConservaSCAD As Boolean = False

    Public Class BackgroundWorker_Crea_STL_Argument
        Public Enum TipoCreaSegmenti
            Per_STL
            Per_SCAD
        End Enum

        Public Tipo As TipoCreaSegmenti
        Public FileStl As String

    End Class

    Dim WithEvents BackgroundWorker_Crea_Segmenti As BackgroundWorker
    Dim WithEvents BackgroundWorker_Crea_Processi As BackgroundWorker
    Dim WithEvents BackgroundWorker_Crea_STL As BackgroundWorker

    Private Sub ButtonLoadGCode_Click(sender As Object, e As EventArgs) Handles ButtonLoadGCode.Click
        TextBoxGCode.Text = String.Empty
        TextBoxOpenScad.Text = String.Empty

        Dim frmX As New OpenFileDialog With {
            .Filter = "*.gcode|*.gcode"
        }

        If frmX.ShowDialog(Me) = DialogResult.OK Then
            GCodeInput = frmX.FileName
            TextBoxGCode.Text = IO.File.ReadAllText(GCodeInput)

        End If

    End Sub

    Private Sub ButtonSaveOpenScad_Click(sender As Object, e As EventArgs) Handles ButtonSaveOpenScad.Click
        Dim frmX As New SaveFileDialog With {
            .Filter = "*.scad|*.scad"
        }

        If frmX.ShowDialog(Me) = DialogResult.OK Then
            Testo($"{vbCrLf}CALCOLO I SEGMENTI ...")

            BackgroundWorker_Crea_Segmenti = New BackgroundWorker() With {
                .WorkerReportsProgress = True,
                .WorkerSupportsCancellation = True
            }

            BackgroundWorker_Crea_Segmenti.RunWorkerAsync(
                New BackgroundWorker_Crea_STL_Argument() With {
                    .Tipo = BackgroundWorker_Crea_STL_Argument.TipoCreaSegmenti.Per_SCAD,
                    .FileStl = frmX.FileName
                }
            )

        End If

    End Sub

    Private Class clsSegmento
        Public Property Livello As Integer = 0
        Public Property Layer As Single = 0
        Public Property Primo As Boolean = True

        Public Class clsXYZ
            Public Property X As Single = 0
            Public Property Y As Single = 0
            Public Property Z As Single = 0

            Public Sub New()
                X = 0
                Y = 0
                Z = 0
            End Sub

            Public Sub New(_x As Single, _y As Single, _z As Single)
                X = _x
                Y = _y
                Z = _z

            End Sub

            Public ReadOnly Property aStringaSemplice As String
                Get
                    Return $"{X.ToString(CultureInfo.InvariantCulture)},{Y.ToString(CultureInfo.InvariantCulture)},{Z.ToString(CultureInfo.InvariantCulture)}"

                End Get
            End Property

            Public ReadOnly Property aStringaComplessa As String
                Get
                    Return $"[{aStringaSemplice()}]"

                End Get
            End Property

        End Class

        Public Property Origine As clsXYZ
        Public Property Destinazione As clsXYZ

        Public ReadOnly Property Angolo As Single
            Get
                If Destinazione.X >= Origine.X Then
                    Return Math.Round(Math.Atan((Destinazione.Y - Origine.Y) / (Destinazione.X - Origine.X)) * 180 / Math.PI, 3)

                Else
                    Return Math.Round((Math.Atan((Origine.Y - Destinazione.Y) / (Origine.X - Destinazione.X)) * 180 / Math.PI) + 180, 3)

                End If

            End Get
        End Property

        Public ReadOnly Property Distanza As Single
            Get
                Return Math.Round(Math.Sqrt((Math.Abs(Destinazione.Y - Origine.Y) ^ 2) + (Math.Abs(Destinazione.X - Origine.X) ^ 2)), 3)

            End Get
        End Property

        Public ReadOnly Property aStringaSemplice As String
            Get
                Return $"{Origine.aStringaSemplice()},{Destinazione.aStringaSemplice()}"

            End Get
        End Property

        Public ReadOnly Property aStringaComplessa As String
            Get
                Return $"[{Origine.aStringaComplessa()},{Destinazione.aStringaComplessa()}]"

            End Get
        End Property

        Public ReadOnly Property aStringaComplessaAD As String
            Get
                Return $"[{Origine.aStringaComplessa()},{Angolo.ToString(CultureInfo.InvariantCulture)},{Distanza.ToString(CultureInfo.InvariantCulture)}]"

            End Get
        End Property

        Public Sub New()
            Livello = 0
            Layer = 0.2

            Origine = New clsXYZ()

            Destinazione = New clsXYZ()

            Primo = True

        End Sub

        Public Sub New(_livello As Integer, _layer As Single, _x1 As Single, _y1 As Single, _z1 As Single, _x2 As Single, _y2 As Single, _z2 As Single)
            Livello = _livello
            Layer = _layer

            Origine = New clsXYZ(_x1, _y1, _z1)

            Destinazione = New clsXYZ(_x2, _y2, _z2)

            Primo = True

        End Sub

    End Class

    'Private Sub GCodeToSegmenti()

    '    Dim dblOX As Single = 0
    '    Dim dblOY As Single = 0
    '    Dim dblOZ As Single = 0
    '    Dim dblOE As Single = 0
    '    Dim dblOF As Single = 0

    '    Dim dblDX As Single = 0
    '    Dim dblDY As Single = 0
    '    Dim dblDZ As Single = 0
    '    Dim dblDE As Single = 0
    '    Dim dblDF As Single = 0

    '    Dim intLi As Integer = 0
    '    Dim dblLa As Single = dblLayer

    '    Dim blnPA As Boolean = True
    '    Dim blnPR As Boolean = True

    '    For Each strR As String In IO.File.ReadAllLines(GCodeInput)
    '        If strR.Contains(";") Then
    '            strR = strR.Split(";")(0)

    '        End If

    '        strR = strR.Trim.ToUpper

    '        If Not String.IsNullOrEmpty(strR) Then

    '            Dim strP() As String = strR.Split(" ")

    '            Select Case strP(0)
    '                Case "G90"
    '                    blnPA = True

    '                Case "G91"
    '                    blnPA = False

    '                Case "G0", "G92"
    '                    For intI As Integer = 1 To strP.Count - 1
    '                        Dim strV As String = strP(intI)

    '                        If strV.StartsWith("X") Then
    '                            dblDX = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
    '                            dblOX = dblDX
    '                            blnPR = True

    '                        ElseIf strV.StartsWith("Y") Then
    '                            dblDY = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
    '                            dblOY = dblDY
    '                            blnPR = True

    '                        ElseIf strV.StartsWith("Z") Then
    '                            dblDZ = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                            If dblOZ < dblDZ Then
    '                                intLi += 1
    '                                dblLa = Math.Abs(dblDZ - dblOZ)
    '                                blnPR = True

    '                            ElseIf dblOZ > dblDZ Then
    '                                intLi = 1
    '                                dblLa = dblDZ
    '                                blnPR = True

    '                            End If

    '                            dblOZ = dblDZ

    '                        ElseIf strV.StartsWith("E") Then
    '                            dblDE = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
    '                            dblOE = dblDE

    '                        ElseIf strV.StartsWith("F") Then
    '                            dblDF = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
    '                            dblOF = dblDF

    '                        End If

    '                    Next

    '                Case "G1"
    '                    For intI As Integer = 1 To strP.Count - 1
    '                        Dim strV As String = strP(intI)

    '                        If strV.StartsWith("X") Then
    '                            dblDX = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                        ElseIf strV.StartsWith("Y") Then
    '                            dblDY = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                        ElseIf strV.StartsWith("Z") Then
    '                            dblDZ = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                        ElseIf strV.StartsWith("E") Then
    '                            dblDE = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                        ElseIf strV.StartsWith("F") Then
    '                            dblDF = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

    '                        End If

    '                    Next

    '                    If dblOZ < dblDZ Then
    '                        intLi += 1
    '                        dblLa = Math.Abs(dblDZ - dblOZ)
    '                        blnPR = True

    '                    ElseIf dblOZ > dblDZ Then
    '                        intLi = 1
    '                        dblLa = dblDZ
    '                        blnPR = True

    '                    End If

    '                    Dim Distanza As Single = Math.Round(Math.Sqrt((Math.Abs(dblDY - dblOY) ^ 2) + (Math.Abs(dblDX - dblOX) ^ 2)), 3)

    '                    If Distanza > 0 AndAlso dblDE > 0 Then
    '                        Segmenti.Add(New clsSegmento() With {
    '                                     .Livello = intLi,
    '                                     .Layer = Math.Round(dblLa, 3),
    '                                     .Origine = New clsSegmento.clsXYZ() With {
    '                                         .X = dblOX,
    '                                         .Y = dblOY,
    '                                         .Z = dblOZ
    '                                     },
    '                                     .Destinazione = New clsSegmento.clsXYZ() With {
    '                                         .X = dblDX,
    '                                         .Y = dblDY,
    '                                         .Z = dblDZ
    '                                     },
    '                                     .Primo = blnPR
    '                                     })

    '                        blnPR = False

    '                    End If

    '                    dblOX = dblDX
    '                    dblOY = dblDY
    '                    dblOZ = dblDZ
    '                    dblOE = dblDE
    '                    dblOF = dblDF

    '            End Select

    '        End If

    '    Next

    'End Sub

    'Private Sub GCodeToOpenScad()

    '    GCodeToSegmenti()

    '    Dim stbR As New StringBuilder()

    '    stbR.AppendLine("$fn=20;")
    '    stbR.AppendLine("")
    '    stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '    stbR.AppendLine($"Layer={dblLayer.ToString(CultureInfo.InvariantCulture)} + 0.001;")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("ModoDrw = 3;")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("module drw(xyz) {")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("    if (ModoDrw==1) {")
    '    stbR.AppendLine("        hull() {")
    '    stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(h=Layer,d=Ugello,center=true);")
    '    stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(h=Layer,d=Ugello,center=true);")
    '    stbR.AppendLine("        };")
    '    stbR.AppendLine("    } else if (ModoDrw==2) {")
    '    stbR.AppendLine("        translate([xyz[3], xyz[4], xyz[5]]) cylinder(h=Layer,d=Ugello,center=true);")
    '    stbR.AppendLine("    } else if (ModoDrw==3) {")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("        dy = abs(xyz[4] - xyz[1]);")
    '    stbR.AppendLine("        dx = abs(xyz[3] - xyz[0]);")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("        distanza = sqrt((dy * dy) + (dx * dx));")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("        if (xyz[3] >= xyz[0]) {")
    '    stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, atan((xyz[4] - xyz[1]) / (xyz[3] - xyz[0]))]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
    '    stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
    '    stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
    '    stbR.AppendLine("        } else {")
    '    stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, (atan((xyz[1] - xyz[4]) / (xyz[0] - xyz[3])) + 180.0)]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
    '    stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
    '    stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
    '    stbR.AppendLine("     }")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("};")
    '    stbR.AppendLine("")
    '    stbR.Append("gcd = [")

    '    Dim blnPrimo As Boolean = True

    '    For Each sg As clsSegmento In Segmenti
    '        If blnPrimo Then
    '            blnPrimo = False

    '        Else
    '            stbR.AppendLine($",")

    '        End If

    '        stbR.Append($"[{sg.aStringaSemplice()}]")

    '    Next

    '    stbR.AppendLine("];")
    '    stbR.AppendLine("")
    '    stbR.AppendLine("union () {")

    '    Dim intS As Integer = 2000
    '    Dim intCnt As Integer = Segmenti.Count - 1

    '    For intF As Integer = 0 To intCnt Step intS
    '        If intF > 0 Then
    '            stbR.Append("//")

    '        End If

    '        stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) drw(gcd[i]);")

    '    Next

    '    stbR.AppendLine("};")

    '    TextBoxOpenScad.Text = stbR.ToString

    'End Sub

    Private Class clsProcesso
        Public Property Processo As Process = Nothing
        Public Property FileSCAD As String = String.Empty
        Public Property Scad As String = String.Empty

        Public ReadOnly Property FileStl As String
            Get
                Return FileSCAD.Replace(".scad", ".stl")
            End Get
        End Property

        Private StartTime As Date = Now

        Public Enum enmStato
            In_Creazione
            Fermo
            Attivo
            Finito
            Finito_Con_Errore
            Eliminato
            Finale
        End Enum

        Public Property Stato As enmStato = enmStato.In_Creazione

        Public Class ProcessoEventArgs
            Inherits EventArgs
            Public Property Testo As String = String.Empty
            Public Sub New(_testo As String)
                Testo = _testo
            End Sub
        End Class

        Public Event Rapporto(sender As Object, e As ProcessoEventArgs)
        Public Event Finito(sender As Object, e As EventArgs)

        Public Sub Avvia()
            Stato = enmStato.Attivo
            StartTime = Now

            Try
                IO.File.WriteAllText(FileSCAD, Scad)
                RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > Elaboro"))

                Processo = New Process With {
                    .EnableRaisingEvents = True
                }

                Processo.StartInfo.FileName = "C:\Program Files\OpenSCAD\openscad.exe"
                Processo.StartInfo.Arguments = $"-o ""{FileStl}"" ""{FileSCAD}"""
                Processo.StartInfo.UseShellExecute = False
                Processo.StartInfo.RedirectStandardOutput = True
                Processo.StartInfo.RedirectStandardError = True

                AddHandler Processo.OutputDataReceived, Sub(ps, pe)
                                                            If pe.Data IsNot Nothing AndAlso Not String.IsNullOrEmpty(pe.Data.Trim) Then
                                                                RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > {pe.Data}"))
                                                            End If
                                                        End Sub

                AddHandler Processo.ErrorDataReceived, Sub(ps, pe)
                                                           If pe.Data IsNot Nothing AndAlso Not String.IsNullOrEmpty(pe.Data.Trim) Then
                                                               RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > {pe.Data}"))
                                                           End If
                                                       End Sub

                AddHandler Processo.Exited, Sub(ps, pe)
                                                RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > finito in {DateDiff(DateInterval.Second, StartTime, Now).ToString()} sec"))
                                                RaiseEvent Finito(Me, New EventArgs())
                                                Stato = enmStato.Finito

                                                Processo.CancelOutputRead()
                                                Processo.CancelErrorRead()
                                                Processo.Dispose()

                                            End Sub

                Processo.Start()
                Processo.BeginOutputReadLine()
                Processo.BeginErrorReadLine()

            Catch ex As Exception
                Stato = enmStato.Finito_Con_Errore
                RaiseEvent Finito(Me, New EventArgs())

            End Try

        End Sub

        Public Sub Uccidi()
            Stato = enmStato.Eliminato

            Try
                Processo.Kill()

            Catch ex As Exception
                ' boh

            End Try

            Processo.Dispose()

        End Sub

    End Class

    Private Sub ButtonCreaSTL_Click(sender As Object, e As EventArgs) Handles ButtonCreaSTL.Click

        TempFolder = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

        If IO.Directory.Exists(TempFolder) Then
            IO.Directory.Delete(TempFolder)
        End If

        IO.Directory.CreateDirectory(TempFolder)

        TextBoxOpenScad.Text = TempFolder

        Testo($"{vbCrLf}CALCOLO I SEGMENTI ...")

        Ugello = NumericUpDownUgello.Value / 10
        GruppoSegmenti = NumericUpDownSegmenti.Value
        GruppoProcessi = NumericUpDownProcessi.Value
        ConservaSTL = CheckBoxConservaSTL.Checked
        ConservaSCAD = CheckBoxConservaSCAD.Checked

        ConteggioSegmentiScadStl = 0

        Dim ifx As New IO.FileInfo(GCodeInput)
        StlOutput = IO.Path.Combine(ifx.DirectoryName, ifx.Name.Replace(ifx.Extension, ".stl"))

        If IO.File.Exists(StlOutput) Then
            IO.File.Delete(StlOutput)

        End If

        ProgressBarStato.Minimum = 0
        ProgressBarStato.Maximum = 1
        ProgressBarStato.Value = 0

        ProgressBarRisultato.Minimum = 0
        ProgressBarRisultato.Maximum = 1
        ProgressBarRisultato.Value = 0

        BackgroundWorker_Crea_Segmenti = New BackgroundWorker() With {
            .WorkerReportsProgress = True,
            .WorkerSupportsCancellation = True
        }

        BackgroundWorker_Crea_Segmenti.RunWorkerAsync(
                New BackgroundWorker_Crea_STL_Argument() With {
                    .Tipo = BackgroundWorker_Crea_STL_Argument.TipoCreaSegmenti.Per_STL,
                    .FileStl = String.Empty
                }
            )

    End Sub

    'Private Sub Modo1()
    '    Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

    '    If IO.Directory.Exists(strTF) Then
    '        IO.Directory.Delete(strTF)
    '    End If

    '    IO.Directory.CreateDirectory(strTF)

    '    TextBoxOpenScad.Text = strTF

    '    GCodeToSegmenti()

    '    Processi = New List(Of clsProcesso)

    '    Dim stbF As New StringBuilder()
    '    stbF.AppendLine("union () {")

    '    For LI As Integer = Segmenti.First.Livello To Segmenti.Last.Livello
    '        Dim stbR As New StringBuilder()

    '        stbR.AppendLine("$fn=25;")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Layer={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("module drw(xyz) {")
    '        stbR.AppendLine("        dy = abs(xyz[4] - xyz[1]);")
    '        stbR.AppendLine("        dx = abs(xyz[3] - xyz[0]);")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("        distanza = sqrt((dy * dy) + (dx * dx));")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("        if (xyz[3] >= xyz[0]) {")
    '        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, atan((xyz[4] - xyz[1]) / (xyz[3] - xyz[0]))]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
    '        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine("        } else {")
    '        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, (atan((xyz[1] - xyz[4]) / (xyz[0] - xyz[3])) + 180.0)]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
    '        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine("     }")
    '        stbR.AppendLine("};")
    '        stbR.AppendLine("")
    '        stbR.Append("gcd = [")

    '        Dim blnPrimo As Boolean = True
    '        Dim intCnt As Integer = 0

    '        For Each sg As clsSegmento In Segmenti
    '            If sg.Livello = LI Then
    '                intCnt += 1

    '                If blnPrimo Then
    '                    blnPrimo = False

    '                Else
    '                    stbR.AppendLine($",")

    '                End If

    '                stbR.Append($"[{sg.aStringaSemplice()}]")

    '            ElseIf sg.Livello > LI Then
    '                Exit For

    '            End If

    '        Next

    '        stbR.AppendLine("];")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("union () {")

    '        Dim intS As Integer = 100

    '        intCnt -= 1

    '        For intF As Integer = 0 To intCnt Step intS
    '            stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) drw(gcd[i]); ")

    '        Next

    '        stbR.AppendLine("};")

    '        Dim FO As String = IO.Path.Combine(strTF, $"L{LI.ToString()}.scad")
    '        IO.File.WriteAllText(FO, stbR.ToString)

    '        Processi.Add(New clsProcesso(FO))

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Testo($"{vbCrLf}{pe.Testo}")
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)
    '                                             ControllaProcessi()
    '                                         End Sub

    '        stbF.AppendLine($"import(""{Processi.Last.FileStl.Replace("\", "/")}"", convexity=5);")

    '    Next

    '    stbF.AppendLine("};")

    '    If True Then
    '        Dim FO As String = IO.Path.Combine(strTF, $"Finale.scad")
    '        IO.File.WriteAllText(FO, stbF.ToString)

    '        Processi.Add(New clsProcesso(FO))
    '        Processi.Last.Stato = clsProcesso.enmStato.Finale

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Testo($"{vbCrLf}{pe.Testo}")
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)
    '                                             ControllaProcessi()
    '                                         End Sub

    '    End If

    '    ControllaProcessi()

    'End Sub

    'Private Sub Modo2()
    '    Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

    '    If IO.Directory.Exists(strTF) Then
    '        IO.Directory.Delete(strTF)
    '    End If

    '    IO.Directory.CreateDirectory(strTF)

    '    TextBoxOpenScad.Text = strTF

    '    GCodeToSegmenti()

    '    Processi = New List(Of clsProcesso)
    '    Dim ListaFile As New List(Of String)

    '    For LI As Integer = 0 To Segmenti.Count - 1
    '        Dim sg As clsSegmento = Segmenti(LI)

    '        Dim stbR As New StringBuilder()

    '        stbR.AppendLine("$fn=25;")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Layer={sg.Layer.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Distanza={sg.Distanza.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Angolo={sg.Angolo.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Origine={sg.Origine.aStringaComplessa()};")
    '        stbR.AppendLine($"Destinazione={sg.Destinazione.aStringaComplessa()};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("union () {")
    '        stbR.AppendLine($"   translate(Origine) rotate([0, 0, Angolo]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([Distanza, Ugello, Layer]);")
    '        stbR.AppendLine($"   translate(Origine) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine($"   translate(Destinazione) cylinder(d=Ugello, h=Layer,center=true);")
    '        stbR.AppendLine("};")

    '        Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")
    '        IO.File.WriteAllText(FO, stbR.ToString)

    '        Processi.Add(New clsProcesso(FO))

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Testo($"{vbCrLf}{pe.Testo}")
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)
    '                                             ControllaProcessi()
    '                                         End Sub

    '        ListaFile.Add($"""{Processi.Last.FileStl.Replace("\", "/")}""")

    '    Next

    '    If True Then
    '        Dim blnPrimo As Boolean = True
    '        Dim intCnt As Integer = 0
    '        Dim stbR As New StringBuilder()

    '        stbR.Append("gcd = [")

    '        For Each strF As String In ListaFile

    '            If blnPrimo Then
    '                blnPrimo = False

    '            Else
    '                stbR.AppendLine($",")

    '            End If

    '            stbR.AppendLine(strF)

    '        Next

    '        stbR.AppendLine("];")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("union () {")

    '        Dim intS As Integer = 100

    '        intCnt = ListaFile.Count - 1

    '        For intF As Integer = 0 To intCnt Step intS
    '            stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) import(gcd[i], convexity=5);")

    '        Next

    '        stbR.AppendLine("};")

    '        Dim FO As String = IO.Path.Combine(strTF, $"Finale.scad")
    '        IO.File.WriteAllText(FO, stbR.ToString)

    '        Processi.Add(New clsProcesso(FO))
    '        Processi.Last.Stato = clsProcesso.enmStato.Finale

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Testo($"{vbCrLf}{pe.Testo}")
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)
    '                                             ControllaProcessi()
    '                                         End Sub

    '    End If

    '    ControllaProcessi()

    'End Sub

    'Dim blnCP As Boolean = False
    'Public Sub ControllaProcessi()
    '    Dim tm As Date = Now.AddSeconds(30)

    '    Do While blnCP AndAlso Now < tm
    '        Threading.Thread.Sleep(1)
    '        My.Application.DoEvents()

    '    Loop

    '    blnCP = True

    '    Dim intCntProcessiAttivi As Integer = 0
    '    Dim intCntProcessiCompletati As Integer = 0

    '    For Each p As clsProcesso In Processi
    '        If p.Stato = clsProcesso.enmStato.Attivo Then
    '            intCntProcessiAttivi += 1

    '        ElseIf p.Stato = clsProcesso.enmStato.Finito Then
    '            intCntProcessiCompletati += 1

    '        End If

    '    Next

    '    Progresso(0, Processi.Count, intCntProcessiCompletati)

    '    For Each p As clsProcesso In Processi
    '        If intCntProcessiAttivi < 10 Then
    '            If p.Stato = clsProcesso.enmStato.Fermo Then
    '                intCntProcessiAttivi += 1
    '                p.Avvia()

    '            End If

    '        Else
    '            Exit For

    '        End If

    '    Next

    '    If intCntProcessiAttivi = 0 AndAlso Processi.Count > 0 Then
    '        If Processi.Last.Stato = clsProcesso.enmStato.Finale Then
    '            Processi.Last.Avvia()

    '        End If

    '    End If

    '    blnCP = False

    'End Sub

    'Private Sub Modo3()
    '    Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

    '    If IO.Directory.Exists(strTF) Then
    '        IO.Directory.Delete(strTF)
    '    End If

    '    IO.Directory.CreateDirectory(strTF)

    '    TextBoxOpenScad.Text = strTF

    '    GCodeToSegmenti()

    '    Processi = New List(Of clsProcesso)

    '    For LI As Integer = 0 To Segmenti.Count - 1
    '        Threading.Thread.Sleep(1)
    '        My.Application.DoEvents()

    '        Dim sg As clsSegmento = Segmenti(LI)

    '        Dim stbR As New StringBuilder()

    '        stbR.AppendLine("$fn=20;")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Layer={sg.Layer.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Distanza={sg.Distanza.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Angolo={sg.Angolo.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine($"Origine={sg.Origine.aStringaComplessa()};")
    '        stbR.AppendLine($"Destinazione={sg.Destinazione.aStringaComplessa()};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("union () {")

    '        If Processi.Count > 0 Then
    '            stbR.AppendLine($"   import(""{Processi.Last.FileStl.Replace("\", "/")}"", convexity=5);")

    '        End If

    '        stbR.AppendLine($"   translate(Origine) rotate([0, 0, Angolo]) translate([0, 0-(Ugello/2), 0]) cube([Distanza, Ugello, Layer]);")
    '        stbR.AppendLine($"   translate(Origine) cylinder(d=Ugello, h=Layer);")
    '        stbR.AppendLine($"   translate(Destinazione) cylinder(d=Ugello, h=Layer);")
    '        stbR.AppendLine("};")

    '        Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")
    '        IO.File.WriteAllText(FO, stbR.ToString)

    '        Processi.Add(New clsProcesso(FO))

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Testo($"{vbCrLf}{pe.Testo}")
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)

    '                                             Dim tm As Date = Now.AddSeconds(30)

    '                                             Do
    '                                                 Threading.Thread.Sleep(1)
    '                                                 My.Application.DoEvents()

    '                                                 If IO.File.Exists(IO.Path.Combine(strTF, $"{Processi.First.FileStl}")) Then

    '                                                     Processi.First.Uccidi()
    '                                                     Processi.RemoveAt(0)

    '                                                     If Processi.Count > 0 Then
    '                                                         Processi.First.Avvia()
    '                                                     End If

    '                                                     Exit Do

    '                                                 End If

    '                                             Loop While Now < tm

    '                                         End Sub

    '    Next

    '    Processi.First.Avvia()

    'End Sub

    'Private Sub Modo4()
    '    Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

    '    If IO.Directory.Exists(strTF) Then
    '        IO.Directory.Delete(strTF)
    '    End If

    '    IO.Directory.CreateDirectory(strTF)

    '    TextBoxOpenScad.Text = strTF

    '    Testo($"{vbCrLf}CALCOLO I SEGMENTI ...")

    '    GCodeToSegmenti()

    '    Processi = New List(Of clsProcesso)

    '    Testo($"{vbCrLf}CREO {Segmenti.Count.ToString()} PROCESSI ...")

    '    For LI As Integer = 0 To Segmenti.Count - 1
    '        ProgressBarStato.Minimum = 0
    '        ProgressBarStato.Maximum = Segmenti.Count
    '        ProgressBarStato.Value = LI + 1

    '        Threading.Thread.Sleep(1)
    '        My.Application.DoEvents()

    '        Dim sg As clsSegmento = Segmenti(LI)
    '        Dim sgn As clsSegmento = Nothing

    '        If Not sg.Primo AndAlso LI < Segmenti.Count - 1 Then
    '            sgn = Segmenti(LI + 1)

    '        End If

    '        Dim stbR As New StringBuilder()

    '        stbR.AppendLine("$fn=20;")
    '        stbR.AppendLine("")

    '        If sgn IsNot Nothing AndAlso Not sgn.Primo Then
    '            stbR.AppendLine("difference () {")

    '        End If

    '        stbR.Append($"translate({sg.Origine.aStringaComplessa()}) rotate([0, 0, {sg.Angolo.ToString(CultureInfo.InvariantCulture)}])") : stbR.AppendLine(" union () {")
    '        stbR.AppendLine($"translate([0, 0-({dblUgello.ToString(CultureInfo.InvariantCulture)}/2), 0]) cube([{sg.Distanza.ToString(CultureInfo.InvariantCulture)}, {dblUgello.ToString(CultureInfo.InvariantCulture)}, {sg.Layer.ToString(CultureInfo.InvariantCulture)}]);")

    '        If sg.Primo Then
    '            stbR.AppendLine($"translate([0,0,0]) cylinder(d={dblUgello.ToString(CultureInfo.InvariantCulture)}, h={sg.Layer.ToString(CultureInfo.InvariantCulture)});")

    '        End If

    '        stbR.AppendLine($"translate([{sg.Distanza.ToString(CultureInfo.InvariantCulture)},0,0]) cylinder(d={dblUgello.ToString(CultureInfo.InvariantCulture)}, h={sg.Layer.ToString(CultureInfo.InvariantCulture)});")
    '        stbR.AppendLine("};")

    '        If sgn IsNot Nothing AndAlso Not sgn.Primo Then
    '            stbR.AppendLine($"translate({sgn.Origine.aStringaComplessa()}) rotate([0, 0, {sgn.Angolo.ToString(CultureInfo.InvariantCulture)}]) translate([0, 0-({dblUgello.ToString(CultureInfo.InvariantCulture)}/2), 0-1]) cube([{sg.Distanza.ToString(CultureInfo.InvariantCulture)}, {dblUgello.ToString(CultureInfo.InvariantCulture)}, {sg.Layer.ToString(CultureInfo.InvariantCulture)}+2]);")
    '            stbR.AppendLine("};")

    '        End If

    '        Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")

    '        Processi.Add(New clsProcesso())

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Log(pe.Testo)
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)

    '                                             Dim p As clsProcesso = CType(ps, clsProcesso)

    '                                             If IO.File.Exists(p.FileStl) Then
    '                                                 IO.File.Delete(p.FileSCAD)

    '                                             Else
    '                                                 Testo($"{vbCrLf}{p.FileStl} > Mancante")
    '                                                 'p.Stato = clsProcesso.enmStato.Fermo

    '                                             End If

    '                                         End Sub

    '        Processi.Last.FileSCAD = FO
    '        Processi.Last.Scad = stbR.ToString
    '        Processi.Last.Stato = clsProcesso.enmStato.Fermo

    '        Log($"PROCESSO {LI.ToString()} > Creato")

    '    Next

    '    Testo($"{vbCrLf}CREO {Segmenti.Count.ToString()} FILE STL")

    '    If String.IsNullOrEmpty(GCodeInput) Then
    '        StlOutput = IO.Path.Combine(New IO.FileInfo(Processi.First.FileStl).DirectoryName, "Risultato.stl")

    '    Else
    '        Dim ifx As New IO.FileInfo(GCodeInput)
    '        StlOutput = IO.Path.Combine(ifx.DirectoryName, ifx.Name.Replace(ifx.Extension, ".stl"))

    '    End If

    '    If IO.File.Exists(StlOutput) Then
    '        IO.File.Delete(StlOutput)

    '    End If

    '    Segmenti.Clear()

    '    TimerControllo.Start()

    'End Sub

    'Private Sub Modo5()
    '    Dim GruppoSegmenti As Integer = TrackBarGruppoSegmenti.Value

    '    'Testo($"{vbCrLf}CALCOLO I SEGMENTI ...")

    '    'GCodeToSegmenti()

    '    'Testo($"{vbCrLf}CALCOLATI {Segmenti.Count.ToString()} SEGMENTI ...")

    '    Processi = New List(Of clsProcesso)

    '    Testo($"{vbCrLf}CREO I PROCESSI ...")

    '    Dim LI As Integer = 0

    '    Do While LI < Segmenti.Count
    '        Dim stbR As New StringBuilder()

    '        stbR.AppendLine("$fn=20;")
    '        stbR.AppendLine("")
    '        stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("module drw(xyz,r,d,a,p){")
    '        stbR.AppendLine("translate(xyz)")
    '        stbR.AppendLine("rotate([0,0,r])")
    '        stbR.AppendLine("union () {")
    '        stbR.AppendLine("translate([0,0-(Ugello/2),0])")
    '        stbR.AppendLine("cube([d,Ugello,a]);")
    '        stbR.AppendLine("if(p==true) {")
    '        stbR.AppendLine("translate([0,0,0])")
    '        stbR.AppendLine("cylinder(d=Ugello,h=a);")
    '        stbR.AppendLine("}")
    '        stbR.AppendLine("translate([d,0,0])")
    '        stbR.AppendLine("difference () {")
    '        stbR.AppendLine("cylinder(d=Ugello,h=a);")
    '        stbR.AppendLine("translate([0-Ugello,0-((Ugello+0.2)/2),0-0.1])")
    '        stbR.AppendLine("cube([Ugello,Ugello+0.2,a+0.2]);")
    '        stbR.AppendLine("};")
    '        stbR.AppendLine("}; ")
    '        stbR.AppendLine("};")
    '        stbR.AppendLine("")
    '        stbR.AppendLine("union () {")

    '        Dim intCNTS As Integer = 0

    '        Do
    '            ProgressBarStato.Minimum = 0
    '            ProgressBarStato.Maximum = Segmenti.Count
    '            ProgressBarStato.Value = LI + 1

    '            Threading.Thread.Sleep(1)
    '            My.Application.DoEvents()

    '            Dim sg As clsSegmento = Segmenti(LI)

    '            intCNTS += 1
    '            Dim sgn As clsSegmento = Nothing

    '            If Not sg.Primo AndAlso LI < Segmenti.Count - 1 AndAlso intCNTS = GruppoSegmenti AndAlso Not Segmenti(LI + 1).Primo Then
    '                sgn = Segmenti(LI + 1)

    '            End If

    '            If sgn IsNot Nothing Then
    '                stbR.AppendLine("difference () {")

    '            End If

    '            stbR.AppendLine($"drw({sg.Origine.aStringaComplessa()},{sg.Angolo.ToString(CultureInfo.InvariantCulture)},{sg.Distanza.ToString(CultureInfo.InvariantCulture)},{sg.Layer.ToString(CultureInfo.InvariantCulture)},{sg.Primo.ToString(CultureInfo.InvariantCulture).ToLower()});")

    '            If sgn IsNot Nothing Then
    '                stbR.AppendLine($"translate({sgn.Origine.aStringaComplessa()})")
    '                stbR.AppendLine($"rotate([0,0,{sgn.Angolo.ToString(CultureInfo.InvariantCulture)}])")
    '                stbR.AppendLine($"translate([0,0-((Ugello+0.2)/2),0-0.1])")
    '                stbR.AppendLine($"cube([{sgn.Distanza.ToString(CultureInfo.InvariantCulture)},Ugello+0.2,{sgn.Layer.ToString(CultureInfo.InvariantCulture)}+0.2]);")
    '                stbR.AppendLine("};")

    '            End If

    '            LI += 1
    '        Loop While LI < Segmenti.Count AndAlso Not Segmenti(LI).Primo AndAlso intCNTS < GruppoSegmenti

    '        stbR.AppendLine("};")

    '        Processi.Add(New clsProcesso() With {
    '            .FileSCAD = IO.Path.Combine(TempFolder, $"S{LI.ToString()}.scad"),
    '            .Scad = stbR.ToString,
    '            .Stato = clsProcesso.enmStato.Fermo
    '        })

    '        AddHandler Processi.Last.Rapporto, Sub(ps, pe)
    '                                               Log(pe.Testo)
    '                                           End Sub

    '        AddHandler Processi.Last.Finito, Sub(ps, pe)

    '                                             Dim p As clsProcesso = CType(ps, clsProcesso)

    '                                             If IO.File.Exists(p.FileStl) Then
    '                                                 IO.File.Delete(p.FileSCAD)

    '                                             Else
    '                                                 Testo($"{vbCrLf}{p.FileStl} > Mancante")
    '                                                 'p.Stato = clsProcesso.enmStato.Fermo

    '                                             End If

    '                                         End Sub

    '        Log($"PROCESSO {Processi.Count.ToString()} > Creato")

    '    Loop

    '    Testo($"{vbCrLf}CREATI {Processi.Count.ToString()} PROCESSI ...")
    '    Testo($"{vbCrLf}CREO {Processi.Count.ToString()} FILE STL")

    '    If String.IsNullOrEmpty(GCodeInput) Then
    '        StlOutput = IO.Path.Combine(New IO.FileInfo(Processi.First.FileStl).DirectoryName, "Risultato.stl")

    '    Else
    '        Dim ifx As New IO.FileInfo(GCodeInput)
    '        StlOutput = IO.Path.Combine(ifx.DirectoryName, ifx.Name.Replace(ifx.Extension, ".stl"))

    '    End If

    '    If IO.File.Exists(StlOutput) Then
    '        IO.File.Delete(StlOutput)

    '    End If

    '    Segmenti.Clear()

    '    TimerControllo.Start()

    'End Sub

    Delegate Sub Testo_Invoker(_Testo As String)
    Public Sub Testo(_Testo As String)
        If InvokeRequired Then
            Try
                Invoke(New Testo_Invoker(AddressOf Testo), {_Testo})

            Catch ex As Exception
                ' :-(

            End Try

        ElseIf Not String.IsNullOrEmpty(_Testo.Trim) Then
            If TextBoxOpenScad.TextLength > TextBoxOpenScad.MaxLength - (_Testo.Length * 2) Then
                TextBoxOpenScad.Clear()

            End If

            TextBoxOpenScad.SelectionStart = TextBoxOpenScad.Text.Length
            TextBoxOpenScad.SelectionLength = 0
            TextBoxOpenScad.SelectedText = _Testo

            TextBoxOpenScad.ScrollToCaret()

        End If

    End Sub

    Delegate Sub Log_Invoker(_Testo As String)
    Public Sub Log(_Testo As String)
        If InvokeRequired Then
            Try
                Invoke(New Log_Invoker(AddressOf Log), {_Testo})

            Catch ex As Exception
                ' :-(

            End Try

        ElseIf Not String.IsNullOrEmpty(_Testo.Trim) Then
            LabelLog.Text = _Testo

        End If

    End Sub

    'Delegate Sub Progresso_Invoker(_min As Integer, _max As Integer, _value As Integer)
    'Public Sub Progresso(_min As Integer, _max As Integer, _value As Integer)
    '    If InvokeRequired Then
    '        Try
    '            Invoke(New Progresso_Invoker(AddressOf Progresso), {_min, _max, _value})

    '        Catch ex As Exception
    '            ' :-(

    '        End Try

    '    Else
    '        ProgressBarStato.Minimum = Math.Min(_min, _max)
    '        ProgressBarStato.Maximum = Math.Max(_min, _max)
    '        ProgressBarStato.Value = Math.Min(_value, _max)

    '    End If

    'End Sub

    'Private Sub prcX_OutputDataReceived(sender As Object, e As DataReceivedEventArgs) Handles prcOpenSCAD.OutputDataReceived
    '    Testo($"{vbCrLf}{e.Data}")
    'End Sub

    'Private Sub prcX_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs) Handles prcOpenSCAD.ErrorDataReceived
    '    Testo($"{vbCrLf}{e.Data}")
    'End Sub

    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If BackgroundWorker_Crea_Segmenti IsNot Nothing Then
            BackgroundWorker_Crea_Segmenti.CancelAsync()

        End If

        If BackgroundWorker_Crea_Processi IsNot Nothing Then
            BackgroundWorker_Crea_Processi.CancelAsync()

        End If

        If BackgroundWorker_Crea_STL IsNot Nothing Then
            BackgroundWorker_Crea_STL.CancelAsync()

        End If

    End Sub

    'Private Sub TimerControllo_Tick(sender As Object, e As EventArgs) Handles TimerControllo.Tick
    '    Dim intCntProcessiAttivi As Integer = 0
    '    Dim intCntProcessiCompletati As Integer = 0

    '    For Each p As clsProcesso In Processi
    '        If Not TimerControllo.Enabled Then
    '            Exit For
    '        End If
    '        If p.Stato = clsProcesso.enmStato.Attivo Then
    '            intCntProcessiAttivi += 1

    '        ElseIf p.Stato = clsProcesso.enmStato.Finito Then
    '            intCntProcessiCompletati += 1

    '        End If
    '    Next

    '    ProgressBarStato.Minimum = 0
    '    ProgressBarStato.Maximum = Processi.Count
    '    ProgressBarStato.Value = intCntProcessiCompletati

    '    For Each p As clsProcesso In Processi
    '        If Not TimerControllo.Enabled Then
    '            Exit For
    '        End If
    '        If intCntProcessiAttivi < TrackBarOpenScad.Value Then
    '            If p.Stato = clsProcesso.enmStato.Fermo Then
    '                intCntProcessiAttivi += 1
    '                p.Avvia()

    '            End If

    '        Else
    '            Exit For

    '        End If

    '    Next

    '    'Do
    '    '    If Not TimerControllo.Enabled Then
    '    '        Exit Sub
    '    '    End If

    '    Threading.Thread.Sleep(1)
    '    My.Application.DoEvents()

    '    If ConteggioSegmentiScadStl < Processi.Count AndAlso Processi(ConteggioSegmentiScadStl).Stato = clsProcesso.enmStato.Finito AndAlso IO.File.Exists(Processi(ConteggioSegmentiScadStl).FileStl) Then
    '        Dim p As clsProcesso = Processi(ConteggioSegmentiScadStl)

    '        If ConteggioSegmentiScadStl = 0 Then
    '            If IO.File.Exists(StlOutput) Then
    '                IO.File.Delete(StlOutput)

    '            End If

    '            Testo($"{vbCrLf}CREO {StlOutput}")

    '            IO.File.AppendAllText(StlOutput, $"solid DVD_solid{vbCrLf}")

    '        End If

    '        Log($"Incorporo {p.FileStl}")

    '        Dim lstS As New List(Of String)(IO.File.ReadAllLines(p.FileStl))
    '        lstS.RemoveAll(Function(x) x.Trim.ToUpper.StartsWith("SOLID") OrElse x.Trim.ToUpper.StartsWith("ENDSOLID") OrElse String.IsNullOrEmpty(x))

    '        IO.File.AppendAllLines(StlOutput, lstS)

    '        'IO.File.Delete(p.FileStl)

    '        ConteggioSegmentiScadStl += 1

    '        ProgressBarRisultato.Minimum = 0
    '        ProgressBarRisultato.Maximum = Processi.Count
    '        ProgressBarRisultato.Value = ConteggioSegmentiScadStl

    '        If ConteggioSegmentiScadStl = Processi.Count Then
    '            IO.File.AppendAllText(StlOutput, $"endsolid DVD_solid{vbCrLf}")
    '            Testo($"{vbCrLf}CHIUDO {StlOutput}")

    '            Testo($"{vbCrLf}FINITO !!")
    '            Log($"FINITO !!")
    '            TimerControllo.Stop()

    '        End If

    '        'Else
    '        '    Exit Do

    '    End If

    '    'Loop


    '    'If intCntProcessiAttivi = 0 AndAlso Processi.Count > 0 Then ' AndAlso Processi.Count = intCntProcessiCompletati Then
    '    '    TimerControllo.Stop()

    '    '    Testo($"{vbCrLf}CREAZIONE FILE STL FINITA ...")

    '    '    Dim strFo As String = IO.Path.Combine(New IO.FileInfo(Processi.First.FileStl).DirectoryName, "Risultato.stl")

    '    '    Testo($"{vbCrLf}INCORPORO I FILE STL IN {strFo}")

    '    '    IO.File.AppendAllText(strFo, $"solid DVD_solid{vbCrLf}")

    '    '    For intI As Integer = 0 To Processi.Count - 1
    '    '        Progresso(0, Processi.Count, intI + 1)

    '    '        Threading.Thread.Sleep(1)
    '    '        My.Application.DoEvents()

    '    '        Dim p As clsProcesso = Processi(intI)

    '    '        If IO.File.Exists(p.FileStl) Then
    '    '            Log($"Incorporo {p.FileStl}")

    '    '            Dim lstS As New List(Of String)(IO.File.ReadAllLines(p.FileStl))
    '    '            lstS.RemoveAll(Function(x) x.Trim.ToUpper.StartsWith("SOLID") OrElse x.Trim.ToUpper.StartsWith("ENDSOLID") OrElse String.IsNullOrEmpty(x))

    '    '            IO.File.AppendAllLines(strFo, lstS)

    '    '            IO.File.Delete(p.FileStl)
    '    '        Else
    '    '            Testo($"{vbCrLf}{p.FileStl} > Non incorporato, assente!")

    '    '        End If

    '    '    Next

    '    '    IO.File.AppendAllText(strFo, $"endsolid DVD_solid{vbCrLf}")

    '    '    Testo($"{vbCrLf}FINITO !!")
    '    '    Log($"FINITO !!")

    '    'End If
    'End Sub

    Private Sub BackgroundWorker_Crea_Segmenti_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker_Crea_Segmenti.DoWork

        If Segmenti Is Nothing Then
            Segmenti = New List(Of clsSegmento)

        Else
            Segmenti.Clear()

        End If

        Dim sndr As BackgroundWorker = CType(sender, BackgroundWorker)

        Dim dblOX As Single = 0
        Dim dblOY As Single = 0
        Dim dblOZ As Single = 0
        Dim dblOE As Single = 0
        Dim dblOF As Single = 0

        Dim dblDX As Single = 0
        Dim dblDY As Single = 0
        Dim dblDZ As Single = 0
        Dim dblDE As Single = 0
        Dim dblDF As Single = 0

        Dim intLi As Integer = 0
        Dim dblLa As Single = dblLayer

        Dim blnPA As Boolean = True
        Dim blnPR As Boolean = True

        For Each strR As String In IO.File.ReadAllLines(GCodeInput)
            If sndr.CancellationPending Then
                Exit For
            End If

            If strR.Contains(";") Then
                strR = strR.Split(";")(0)

            End If

            strR = strR.Trim.ToUpper

            If Not String.IsNullOrEmpty(strR) Then

                Dim strP() As String = strR.Split(" ")

                Select Case strP(0)
                    Case "G90"
                        blnPA = True

                    Case "G91"
                        blnPA = False

                    Case "G0", "G92"
                        For intI As Integer = 1 To strP.Count - 1
                            Dim strV As String = strP(intI)

                            If strV.StartsWith("X") Then
                                dblDX = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOX = dblDX
                                blnPR = True

                            ElseIf strV.StartsWith("Y") Then
                                dblDY = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOY = dblDY
                                blnPR = True

                            ElseIf strV.StartsWith("Z") Then
                                dblDZ = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                                If dblOZ < dblDZ Then
                                    intLi += 1
                                    dblLa = Math.Abs(dblDZ - dblOZ)
                                    blnPR = True

                                ElseIf dblOZ > dblDZ Then
                                    intLi = 1
                                    dblLa = dblDZ
                                    blnPR = True

                                End If

                                dblOZ = dblDZ

                            ElseIf strV.StartsWith("E") Then
                                dblDE = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOE = dblDE

                            ElseIf strV.StartsWith("F") Then
                                dblDF = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOF = dblDF

                            End If

                        Next

                    Case "G1"
                        For intI As Integer = 1 To strP.Count - 1
                            Dim strV As String = strP(intI)

                            If strV.StartsWith("X") Then
                                dblDX = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("Y") Then
                                dblDY = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("Z") Then
                                dblDZ = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("E") Then
                                dblDE = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("F") Then
                                dblDF = Single.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            End If

                        Next

                        If dblOZ < dblDZ Then
                            intLi += 1
                            dblLa = Math.Abs(dblDZ - dblOZ)
                            blnPR = True

                        ElseIf dblOZ > dblDZ Then
                            intLi = 1
                            dblLa = dblDZ
                            blnPR = True

                        End If

                        Dim Distanza As Single = Math.Round(Math.Sqrt((Math.Abs(dblDY - dblOY) ^ 2) + (Math.Abs(dblDX - dblOX) ^ 2)), 3)

                        If Distanza > 0 AndAlso dblDE > 0 Then
                            Segmenti.Add(New clsSegmento() With {
                                         .Livello = intLi,
                                         .Layer = Math.Round(dblLa, 3),
                                         .Origine = New clsSegmento.clsXYZ() With {
                                             .X = dblOX,
                                             .Y = dblOY,
                                             .Z = dblOZ
                                         },
                                         .Destinazione = New clsSegmento.clsXYZ() With {
                                             .X = dblDX,
                                             .Y = dblDY,
                                             .Z = dblDZ
                                         },
                                         .Primo = blnPR
                                         })

                            blnPR = False

                        End If

                        dblOX = dblDX
                        dblOY = dblDY
                        dblOZ = dblDZ
                        dblOE = dblDE
                        dblOF = dblDF

                End Select

            End If

        Next

        e.Cancel = sndr.CancellationPending
        e.Result = e.Argument

    End Sub

    Private Sub BackgroundWorker_Crea_Segmenti_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker_Crea_Segmenti.RunWorkerCompleted
        Dim arg As BackgroundWorker_Crea_STL_Argument = CType(e.Result, BackgroundWorker_Crea_STL_Argument)

        Testo($"{vbCrLf}CALCOLATI {Segmenti.Count.ToString()} SEGMENTI ...")

        If e.Cancelled Then
            Testo($"{vbCrLf}TERMINATO !!")
            Log($"TERMINATO !!")

        ElseIf arg.Tipo = BackgroundWorker_Crea_STL_Argument.TipoCreaSegmenti.Per_STL Then
            Testo($"{vbCrLf}CREO I PROCESSI ...")

            BackgroundWorker_Crea_Processi = New BackgroundWorker() With {
                .WorkerReportsProgress = True,
                .WorkerSupportsCancellation = True
            }

            BackgroundWorker_Crea_Processi.RunWorkerAsync()

        ElseIf arg.Tipo = BackgroundWorker_Crea_STL_Argument.TipoCreaSegmenti.Per_SCAD Then
            Dim stbR As New StringBuilder()

            stbR.AppendLine("$fn=20;")
            stbR.AppendLine("")
            stbR.AppendLine($"Ugello={Ugello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine("module drw(xyz,a) {")
            stbR.AppendLine(" hull() {")
            stbR.AppendLine("  translate(xyz[0]) cylinder(h=a,d=Ugello,center=true);")
            stbR.AppendLine("  translate(xyz[1]) cylinder(h=a,d=Ugello,center=true);")
            stbR.AppendLine(" };")
            stbR.AppendLine("};")
            stbR.AppendLine("")
            stbR.Append("gcd = [")

            Dim blnPrimo As Boolean = True

            For Each sg As clsSegmento In Segmenti
                If blnPrimo Then
                    blnPrimo = False

                Else
                    stbR.AppendLine($",")

                End If

                stbR.Append($"[{sg.aStringaComplessa()},{sg.Layer.ToString(CultureInfo.InvariantCulture)}]")

            Next

            stbR.AppendLine("];")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")

            Dim intS As Integer = 2000
            Dim intCnt As Integer = Segmenti.Count - 1

            For intF As Integer = 0 To intCnt Step intS
                If intF > 0 Then
                    stbR.Append("//")

                End If

                stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) drw(gcd[i][0],gcd[i][1]);")

            Next

            stbR.AppendLine("};")

            IO.File.WriteAllText(arg.FileStl, stbR.ToString)

            Testo($"{vbCrLf}FILE {arg.FileStl} SALVATO !")

        End If

    End Sub

    Private Sub BackgroundWorker_Crea_Processi_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker_Crea_Processi.DoWork
        If Processi Is Nothing Then
            Processi = New List(Of clsProcesso)

        Else
            Processi.Clear()

        End If

        Dim sndr As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim LI As Integer = 0

        Do While LI < Segmenti.Count
            If sndr.CancellationPending Then
                Exit Do
            End If

            Dim stbR As New StringBuilder()

            stbR.AppendLine($"Ugello={Ugello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine("$fn=20;")
            stbR.AppendLine("")
            stbR.AppendLine("module drw(xyz,r,d,a,p){")
            stbR.AppendLine(" translate(xyz)")
            stbR.AppendLine("  rotate([0,0,r])")
            stbR.AppendLine("   hull () {")
            stbR.AppendLine("    if(p) {")
            stbR.AppendLine("     cylinder(d=Ugello,h=a);")
            stbR.AppendLine("    } else {")
            stbR.AppendLine("     translate([0,0-(Ugello/2),0])")
            stbR.AppendLine("      cube([Ugello,Ugello,a]);")
            stbR.AppendLine("    }")
            stbR.AppendLine("    translate([d,0,0])")
            stbR.AppendLine("     cylinder(d=Ugello,h=a);")
            stbR.AppendLine("   };")
            stbR.AppendLine("};")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")

            Dim intCNTS As Integer = 0

            Do
                If sndr.CancellationPending Then
                    Exit Do
                End If

                sndr.ReportProgress(LI)

                Dim sg As clsSegmento = Segmenti(LI)

                intCNTS += 1
                Dim sgn As clsSegmento = Nothing

                If Not sg.Primo AndAlso LI < Segmenti.Count - 1 AndAlso intCNTS = GruppoSegmenti AndAlso Not Segmenti(LI + 1).Primo Then
                    sgn = Segmenti(LI + 1)

                End If

                If sgn IsNot Nothing Then
                    stbR.AppendLine(" difference () {")

                End If

                stbR.AppendLine($"  drw({sg.Origine.aStringaComplessa()},{sg.Angolo.ToString(CultureInfo.InvariantCulture)},{sg.Distanza.ToString(CultureInfo.InvariantCulture)},{sg.Layer.ToString(CultureInfo.InvariantCulture)},{sg.Primo.ToString(CultureInfo.InvariantCulture).ToLower()});")

                If sgn IsNot Nothing Then
                    stbR.AppendLine($"  translate({sgn.Origine.aStringaComplessa()})")
                    stbR.AppendLine($"   rotate([0,0,{sgn.Angolo.ToString(CultureInfo.InvariantCulture)}])")
                    stbR.AppendLine($"    translate([0,0-((Ugello+0.2)/2),0-0.1])")
                    stbR.AppendLine($"     cube([{sgn.Distanza.ToString(CultureInfo.InvariantCulture)},Ugello+0.2,{sgn.Layer.ToString(CultureInfo.InvariantCulture)}+0.2]);")
                    stbR.AppendLine(" };")

                End If

                LI += 1
            Loop While LI < Segmenti.Count AndAlso Not Segmenti(LI).Primo AndAlso intCNTS < GruppoSegmenti

            stbR.AppendLine("};")
            stbR.AppendLine("")

            Processi.Add(New clsProcesso() With {
                .FileSCAD = IO.Path.Combine(TempFolder, $"S{LI.ToString()}.scad"),
                .Scad = stbR.ToString,
                .Stato = clsProcesso.enmStato.Fermo
            })

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Log(pe.Testo)
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)

                                                 Dim p As clsProcesso = CType(ps, clsProcesso)

                                                 If IO.File.Exists(p.FileStl) Then
                                                     If Not ConservaSCAD Then
                                                         IO.File.Delete(p.FileSCAD)

                                                     End If

                                                 Else
                                                     Testo($"{vbCrLf}{p.FileStl} > Mancante")
                                                     'p.Stato = clsProcesso.enmStato.Fermo

                                                 End If

                                             End Sub

            sndr.ReportProgress(-1)

        Loop

        e.Cancel = sndr.CancellationPending

    End Sub

    Private Sub BackgroundWorker_Crea_Processi_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker_Crea_Processi.RunWorkerCompleted

        Testo($"{vbCrLf}CREATI {Processi.Count.ToString()} PROCESSI ...")

        If e.Cancelled Then
            Testo($"{vbCrLf}TERMINATO !!")
            Log($"TERMINATO !!")

        Else
            Segmenti.Clear()

            Testo($"{vbCrLf}CREO {StlOutput}")

            IO.File.AppendAllText(StlOutput, $"solid DVD_solid{vbCrLf}")

            Testo($"{vbCrLf}CREO {Processi.Count.ToString()} FILE STL")

            BackgroundWorker_Crea_STL = New BackgroundWorker() With {
                .WorkerReportsProgress = True,
                .WorkerSupportsCancellation = True
            }

            BackgroundWorker_Crea_STL.RunWorkerAsync()

        End If

    End Sub

    Private Sub BackgroundWorker_Crea_Processi_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker_Crea_Processi.ProgressChanged

        If e.ProgressPercentage >= 0 Then
            ProgressBarStato.Minimum = 0
            ProgressBarStato.Maximum = Segmenti.Count
            ProgressBarStato.Value = e.ProgressPercentage + 1

        Else
            Log($"PROCESSO {Processi.Count.ToString()} > Creato")

        End If

        'Threading.Thread.Sleep(1)
        'My.Application.DoEvents()

    End Sub

    Private Sub BackgroundWorker_Crea_STL_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker_Crea_STL.DoWork
        Dim sndr As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim intCntProcessiAttivi As Integer = 0
        Dim intCntProcessiCompletati As Integer = 0

        For Each p As clsProcesso In Processi
            If sndr.CancellationPending Then
                Exit For
            End If
            If p.Stato = clsProcesso.enmStato.Attivo Then
                intCntProcessiAttivi += 1

            ElseIf p.Stato = clsProcesso.enmStato.Finito Then
                intCntProcessiCompletati += 1

            End If
        Next

        sndr.ReportProgress(intCntProcessiCompletati * (-1))

        For Each p As clsProcesso In Processi
            If sndr.CancellationPending Then
                Exit For
            End If
            If intCntProcessiAttivi < GruppoProcessi Then
                If p.Stato = clsProcesso.enmStato.Fermo Then
                    intCntProcessiAttivi += 1
                    p.Avvia()

                End If

            Else
                Exit For

            End If

        Next

        If ConteggioSegmentiScadStl < Processi.Count AndAlso Processi(ConteggioSegmentiScadStl).Stato = clsProcesso.enmStato.Finito AndAlso IO.File.Exists(Processi(ConteggioSegmentiScadStl).FileStl) Then
            Dim p As clsProcesso = Processi(ConteggioSegmentiScadStl)

            Log($"Incorporo {p.FileStl}")

            Dim lstS As New List(Of String)(IO.File.ReadAllLines(p.FileStl))
            lstS.RemoveAll(Function(x) x.Trim.ToUpper.StartsWith("SOLID") OrElse x.Trim.ToUpper.StartsWith("ENDSOLID") OrElse String.IsNullOrEmpty(x))

            IO.File.AppendAllLines(StlOutput, lstS)

            If Not ConservaSTL Then
                IO.File.Delete(p.FileStl)

            End If

            ConteggioSegmentiScadStl += 1

            sndr.ReportProgress(ConteggioSegmentiScadStl)

        End If

        e.Result = (ConteggioSegmentiScadStl = Processi.Count)
        e.Cancel = sndr.CancellationPending

    End Sub

    Private Sub BackgroundWorker_Crea_STL_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker_Crea_STL.RunWorkerCompleted
        If CType(e.Result, Boolean) Then
            IO.File.AppendAllText(StlOutput, $"endsolid DVD_solid{vbCrLf}")
            Testo($"{vbCrLf}CHIUDO {StlOutput}")

            Testo($"{vbCrLf}FINITO !!")
            Log($"FINITO !!")

        ElseIf e.Cancelled Then
            For Each p As clsProcesso In Processi
                If p.Stato = clsProcesso.enmStato.Attivo Then
                    p.Uccidi()

                End If

            Next

            Testo($"{vbCrLf}TERMINATO !!")
            Log($"TERMINATO !!")

        Else
            CType(sender, BackgroundWorker).RunWorkerAsync()

        End If

    End Sub

    Private Sub BackgroundWorker_Crea_STL_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker_Crea_STL.ProgressChanged

        If e.ProgressPercentage >= 0 Then
            ProgressBarRisultato.Minimum = 0
            ProgressBarRisultato.Maximum = Processi.Count
            ProgressBarRisultato.Value = e.ProgressPercentage

        Else
            ProgressBarStato.Minimum = 0
            ProgressBarStato.Maximum = Processi.Count
            ProgressBarStato.Value = Math.Abs(e.ProgressPercentage)

        End If

        'Threading.Thread.Sleep(1)
        'My.Application.DoEvents()

    End Sub

    'Private Sub prcOpenSCAD_Exited(sender As Object, e As EventArgs) Handles prcOpenSCAD.Exited
    '    Testo($"{vbCrLf}{FileScad.First} > Finito!")

    '    FileScad.RemoveAt(0)

    '    If FileScad.Count > 0 Then
    '        Testo($"{vbCrLf}{FileScad.First} > Elaborazione ...")

    '        prcOpenSCAD.Close()
    '        prcOpenSCAD.StartInfo.Arguments = $"-o ""{FileScad.First}.stl"" ""{FileScad.First}"""
    '        prcOpenSCAD.Start()
    '        prcOpenSCAD.BeginOutputReadLine()
    '        prcOpenSCAD.BeginErrorReadLine()

    '    Else
    '        prcOpenSCAD.Dispose()

    '    End If

    'End Sub

End Class