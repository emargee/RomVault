/******************************************************
 *     ROMVault2 is written by Gordon J.              *
 *     Contact gordon@romvault.com                    *
 *     Copyright 2014                                 *
 ******************************************************/

using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ROMVault2.RvDB;
using ROMVault2.SupportedFiles;

namespace ROMVault2
{

    public class RomCellDraw : DataGridViewImageCell
    {
        private readonly Color _bgCol;
        private readonly string _bitmapName;

        public RomCellDraw(string name,Color bgCol)
        {
            _bitmapName = name;
            _bgCol = bgCol;

        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            graphics.FillRectangle(new SolidBrush(_bgCol), cellBounds);

            Bitmap bm= rvImages.GetBitmap(_bitmapName);

            if (bm != null)
            {
                graphics.DrawImage(bm, cellBounds.Left, cellBounds.Top, bm.Width,bm.Height);
                bm.Dispose();
            }

            
            else
                Debug.WriteLine("Missing Graphic for " + _bitmapName);
        }

    }

    public class DirCellDraw : DataGridViewImageCell
    {
        private readonly Color _bgCol;
        private readonly RvDir _cellDir;

        public DirCellDraw(RvDir cellDir, Color bgCol)
        {
            _cellDir = cellDir;
            _bgCol = bgCol;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {

            graphics.FillRectangle(new SolidBrush(_bgCol), cellBounds);

            string bitmapName;

            switch (_cellDir.FileType)
            {
                case FileType.Zip:
                    if (_cellDir.RepStatus == RepStatus.DirCorrect && _cellDir.ZipStatus==ZipStatus.TrrntZip)
                        bitmapName = "ZipTZ";
                    else
                        bitmapName = "Zip" + _cellDir.RepStatus;
                    break;
                default:
                    bitmapName = "Dir" + _cellDir.RepStatus;
                    break;
            }

            Bitmap bm = rvImages.GetBitmap(bitmapName);

            if (bm != null)
            {
                graphics.DrawImage(bm, cellBounds.Left + 10, cellBounds.Top, 21, 17);
                bm.Dispose();
            }
            else
                Debug.WriteLine("Missing Graphic for " + bitmapName);
        }
    }

    public class DirCellStatusDraw : DataGridViewImageCell
    {
        private readonly RvDir _dir;
        public DirCellStatusDraw(RvDir dir)
        {
            _dir = dir;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Font drawFont = new Font("Arial", 9);
            SolidBrush drawBrushBlack = new SolidBrush(Color.Black);
            SolidBrush drawBrushWhite = new SolidBrush(Color.White);

            graphics.FillRectangle(drawBrushWhite, cellBounds);

            int gOff;
            int columnIndex = 0;
            for (int l = 0; l < RepairStatus.DisplayOrder.Length; l++)
            {
                if (l >= 13) columnIndex = l;

                if (_dir.DirStatus.Get(RepairStatus.DisplayOrder[l]) <= 0) continue;

                gOff = cellBounds.Left + FrmMain.GameGridColumnXPositions[columnIndex];
                Bitmap bm = rvImages.GetBitmap(@"G_" + RepairStatus.DisplayOrder[l]);
                if (bm != null)
                {
                    graphics.DrawImage(bm, gOff, cellBounds.Top, 21, 18);
                    bm.Dispose();
                }
                else
                    Debug.WriteLine("Missing Graphics for " + "G_" + RepairStatus.DisplayOrder[l]);

                columnIndex++;
            }

            columnIndex = 0;
            for (int l = 0; l < RepairStatus.DisplayOrder.Length; l++)
            {
                if (l >= 13)
                    columnIndex = l;

                if (_dir.DirStatus.Get(RepairStatus.DisplayOrder[l]) > 0)
                {
                    gOff = cellBounds.Left + FrmMain.GameGridColumnXPositions[columnIndex];
                    graphics.DrawString(_dir.DirStatus.Get(RepairStatus.DisplayOrder[l]).ToString(CultureInfo.InvariantCulture), drawFont, drawBrushBlack, new PointF(gOff + 20, cellBounds.Top + 3));
                    columnIndex++;
                }
            }
            drawBrushBlack.Dispose();
            drawBrushWhite.Dispose();
            drawFont.Dispose();
        }
    }
}