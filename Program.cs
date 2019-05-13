using System;
using System.Text;
using Gdk;
using Gtk;

namespace ColourizmusS
{
    class Program
    {
        private static ColorSelection _colorSelection;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            ColorSelectionDialog colorSelectionDialog = new ColorSelectionDialog("Colourizmus S+");

            colorSelectionDialog.Close += CloseProgram;
            colorSelectionDialog.DeleteEvent += CloseProgram;

            colorSelectionDialog.OkButton.Clicked += CopyToClipboard;

            colorSelectionDialog.CancelButton.Hide();
            colorSelectionDialog.OkButton.Label = "Copy to clipboard";

            _colorSelection = colorSelectionDialog.ColorSelection;
            _colorSelection.ColorChanged += CopyToClipboard;

            colorSelectionDialog.Show();
            Application.Run();
        }

        private static void CloseProgram(object sender, EventArgs e)
        {
            Application.Quit();
        }

        private static void CopyToClipboard(object sender, EventArgs e)
        {
            Clipboard.GetDefault(Display.Default).Text = HexFormat(_colorSelection.CurrentRgba);
        }

        private static string HexFormat(RGBA color)
        {
            StringBuilder sb = new StringBuilder();
            double[] vals = {color.Red, color.Green, color.Blue};
            char[] hexchars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

            sb.Append('#');
            foreach (double val in vals)
            {
                /* Convert to a range of 0-255, then lookup the
                 * digit for each half-byte */
                byte rounded = (byte) (val * 255);
                sb.Append(hexchars[(rounded & 0xf0) >> 4]);
                sb.Append(hexchars[rounded & 0x0f]);
            }

            return sb.ToString();
        }
    }
}