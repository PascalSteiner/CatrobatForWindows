﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Catrobat.Paint.Data;
using Coding4Fun.Toolkit.Controls.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;

namespace Catrobat.Paint.ViewModel
{
    public class PaintingAreaViewModel : ViewModelBase
    {
        private readonly Stack<Stroke> _undoneStrokes = new Stack<Stroke>();
        private Stroke _stroke;

        private readonly StrokeCollection _strokes = new StrokeCollection();

        public StrokeCollection Strokes
        {
            get
            {
                return _strokes;
            }
        }

        public PaintingAreaViewModel()
        {
            BeginStrokeCommand = new RelayCommand<Point>(BeginStrokeExecute);
            SetStrokePointCommand = new RelayCommand<Point>(SetStrokePointExecute);
            EndStrokeCommand = new RelayCommand(EndStrokeExecute);
            ClearCommand = new RelayCommand(ClearExecute);
            UndoCommand = new RelayCommand(UndoExecute);
            RedoCommand = new RelayCommand(RedoExecute);
            SaveCommand = new RelayCommand<WriteableBitmap>(SaveExecute);
            ToColorPickerCommand = new RelayCommand(ToColorPickerExecute);
        }

        ~PaintingAreaViewModel()
        {
            Debug.WriteLine("PaintingAreaViewModel Destructor called.");
        }
 

        #region Commands

        public ICommand ToColorPickerCommand { get; private set; }
        private void ToColorPickerExecute()
        {
            var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneApplicationFrame != null)
                phoneApplicationFrame.Navigate(new Uri("/Paint;component/View/ColorPickerView.xaml", UriKind.RelativeOrAbsolute));
        }

        public ICommand SaveCommand { get; private set; }
        private void SaveExecute(WriteableBitmap wb)
        {
            PaintLauncher.Task.CurrentImage = new WriteableBitmap(wb);
            PaintLauncher.Task.RaiseImageChanged();

            //using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(DateTime.Now.ToLongDateString()  + ".jpg", FileMode.Create, FileAccess.Write))
            //    {
            //        wb.SaveJpeg(targetStream, wb.PixelWidth, wb.PixelHeight, 1, 100);
            //        targetStream.Close();
            //        var biMap = new BitmapImage();
            //        biMap.CreateOptions = BitmapCreateOptions.None;
            //        using (var fs = isStore.OpenFile(DateTime.Now.ToLongDateString() + ".jpg", FileMode.Open))
            //        {
            //            biMap.SetSource(fs);
            //            PaintLauncher.Task.CurrentImage = biMap;
            //            PaintLauncher.Task.RaiseImageChanged();
            //            MessageBox.Show(Resources.AppResources.PaintingAreaMessageBoxImageSaved + " " + Path.GetFileName(DateTime.Now.ToLongDateString() + ".jpg"));
            //        }
                    
            //    }
            //} 
        }


        public ICommand BeginStrokeCommand { get; private set; }
        private void BeginStrokeExecute(Point point)
        {
            _stroke = new Stroke();
            _stroke.StylusPoints.Add(ConvertToStylusPoint(point));
            _stroke.DrawingAttributes.Color = GlobalValues.Instance.SelectedColorAsColor;
            _strokes.Add(_stroke);
            Debug.WriteLine("<StylusPoint X=\"{0}\" Y=\"{1}\" />", point.X, point.Y);

        }

        public ICommand SetStrokePointCommand { get; private set; }
        private void SetStrokePointExecute(Point point)
        {
            if (_stroke != null)
            {
                _stroke.StylusPoints.Add(ConvertToStylusPoint(point));
                Debug.WriteLine("<StylusPoint X=\"{0}\" Y=\"{1}\" />", point.X, point.Y);
            }
        }

        public ICommand EndStrokeCommand { get; private set; }
        private void EndStrokeExecute()
        {
            _stroke = null;
        }

        public ICommand ClearCommand { get; private set; }
        private void ClearExecute()
        {
            _strokes.Clear();
        }

        public ICommand UndoCommand { get; private set; }
        private void UndoExecute()
        {
            if (_strokes.Count > 0)
            {
                _undoneStrokes.Push(_strokes.Last());
                _strokes.RemoveAt(_strokes.Count - 1);
            }
        }

        public ICommand RedoCommand { get; private set; }
        private void RedoExecute()
        {
            if (_undoneStrokes.Count > 0)
            {
                _strokes.Add(_undoneStrokes.Pop());
            }
        }
        #endregion




        static StylusPoint ConvertToStylusPoint(Point position)
        {
            return new StylusPoint(position.X, position.Y);
        }
    }
}