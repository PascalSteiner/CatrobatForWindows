﻿using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// Die Elementvorlage "Benutzersteuerelement" ist unter http://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace Catrobat.Paint.WindowsPhone.Controls.UserControls
{
    public sealed partial class RectangleSelectionControl : UserControl
    {
        TransformGroup _transformGridMain;
        bool _isModifiedRectangleMovement;
        double _rectangleForMovementSize = 200.0;
        double _rectangleToDrawSize = 160.0;
        double _gridMainSize = 290.0;
        public Grid m_mainGrid = null;

        private Boolean m_isRotating = false;

        public RectangleSelectionControl()
        {
            this.InitializeComponent();

            GridMainSelection.RenderTransform = _transformGridMain = new TransformGroup();

            //rectRectangleToDraw.Fill = PocketPaintApplication.GetInstance().PaintData.colorSelected;
            //rectRectangleToDraw.Stroke = PocketPaintApplication.GetInstance().PaintData.strokeColorSelected;
            //rectRectangleToDraw.StrokeThickness = PocketPaintApplication.GetInstance().PaintData.strokeThickness;

            isModifiedRectangleMovement = false;

            m_mainGrid = GridMainSelection;
        }

        public bool isModifiedRectangleMovement
        {
            get
            {
                return _isModifiedRectangleMovement;
            }
            set
            {
                _isModifiedRectangleMovement = value;
            }
        }

        public Rectangle rectangleToDraw
        {
            get
            {
                return rectRectangleToDraw;
            }
            set
            {
                rectRectangleToDraw = value;
            }
        }

        public Rectangle rectangleForMovement
        {
            get
            {
                return rectRectangleForMovement;
            }
            set
            {
                rectRectangleForMovement = value;
            }
        }

        public PenLineJoin strokeLineJoinOfRectangleToDraw
        {
            get
            {
                return rectRectangleToDraw.StrokeLineJoin;
            }
            set
            {
                rectRectangleToDraw.StrokeLineJoin = value;
            }
        }


        public bool shouldHeightOfSelectionChanged(double heightOfSelection)
        {
            bool result = heightOfSelection >= 1.0 ? true : false;
            return result;
        }
        public bool shouldWidthOfSelectionChanged(double widthOfSelection)
        {
            bool result = widthOfSelection >= 1.0 ? true : false;
            return result;
        }

        private void ellLeftTop_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = deltaTranslation.X * -1.0;
            double deltaTranslationY = deltaTranslation.Y * -1.0;
            double newHeightOfRectangleToDraw = rectangleToDraw.Height + deltaTranslationY;

            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                    GridMainSelection.Margin.Top - deltaTranslationY,
                    GridMainSelection.Margin.Right,
                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }

            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(newWidthOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left - deltaTranslationX,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }

        private void TopCenterGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationY = (deltaTranslation.Y) * -1.0;
            double newHeightOfRectangleToDraw = heightOfRectangleToDraw + deltaTranslationY;
            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                    GridMainSelection.Margin.Top - deltaTranslationY,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }
        }

        private void TopRightGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = deltaTranslation.X;
            double deltaTranslationY = deltaTranslation.Y * -1.0;
            double newHeightOfRectangleToDraw = rectangleToDraw.Height + deltaTranslationY;
            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                    GridMainSelection.Margin.Top - deltaTranslationY,
                    GridMainSelection.Margin.Right,
                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }

            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(newWidthOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right - deltaTranslationX,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }

        private void CenterRightGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = deltaTranslation.X;
            double newWidthRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthRectangleToDraw))
            {
                SetWidthOfControl(newWidthRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                                GridMainSelection.Margin.Top,
                                                GridMainSelection.Margin.Right - deltaTranslationX,
                                                GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthRectangleToDraw;
            }
        }

        private void BottomRightGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = deltaTranslation.X;
            double deltaTranslationY = deltaTranslation.Y;
            double newHeightOfRectangleToDraw = heightOfRectangleToDraw + deltaTranslationY;

            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                GridMainSelection.Margin.Top,
                                GridMainSelection.Margin.Right,
                                GridMainSelection.Margin.Bottom - deltaTranslation.Y);
                SetHeightOfControl(newHeightOfRectangleToDraw);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }

            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(newWidthOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right - deltaTranslationX,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }

        private void BottomCenterGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationY = deltaTranslation.Y;
            double newHeightOfRectangleToDraw = heightOfRectangleToDraw + deltaTranslationY;
            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom - deltaTranslationY);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }
        }

        private void BottomLeftGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = (deltaTranslation.X) * -1.0;
            double deltaTranslationY = deltaTranslation.Y;
            double newHeightOfRectangleToDraw = heightOfRectangleToDraw + deltaTranslationY;

            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                                GridMainSelection.Margin.Top,
                                GridMainSelection.Margin.Right,
                                GridMainSelection.Margin.Bottom - deltaTranslationY);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }

            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(rectangleToDraw.Width + deltaTranslationX);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left - deltaTranslationX,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }

        private void CenterLeftGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = (deltaTranslation.X) * -1.0;
            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(rectangleToDraw.Width + deltaTranslationX);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left - deltaTranslationX,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }

        private void rectEllipseForMovement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TranslateTransform translateTransform = new TranslateTransform();

            TranslateTransform lastTranslateTransform = getLastTranslateTransformation();
            RotateTransform lastRotateTransform = getLastRotateTransformation();

            double xVal = e.Delta.Translation.X;
            double yVal = e.Delta.Translation.Y;           

            if (lastTranslateTransform != null && lastRotateTransform == null)
            {
                translateTransform.X = xVal + lastTranslateTransform.X;
                translateTransform.Y = yVal + lastTranslateTransform.Y;
            }
            else if (lastTranslateTransform == null && lastRotateTransform != null)
            {
                translateTransform.X = xVal;
                translateTransform.Y = yVal;
            }
            else if (lastTranslateTransform != null && lastRotateTransform != null)
            {
                translateTransform.X = xVal + lastTranslateTransform.X;
                translateTransform.Y = yVal + lastTranslateTransform.Y;
            }
            else
            {
                translateTransform.X = xVal;
                translateTransform.Y = yVal;
            }
            addTransformation(translateTransform);
        }

        private void rectEllipseForMovement_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var coord = e.GetPosition(PocketPaintApplication.GetInstance().GridWorkingSpace);
            var coord2 = e.GetPosition(rectangleToDraw);

            var angle = PocketPaintApplication.GetInstance().angularDegreeOfWorkingSpaceRotation;

            switch(angle)
            {
                case 0:
                    coord.X -= coord2.X;
                    coord.Y -= coord2.Y;
                    coord.X += (rectangleToDraw.Width / 2);
                    coord.Y += (rectangleToDraw.Height / 2);
                    break;
                case 90:
                    coord.X -= coord2.Y;
                    coord.Y -= (rectangleToDraw.Width - coord2.X);
                    coord.X += (rectangleToDraw.Width / 2);
                    coord.Y += (rectangleToDraw.Height / 2);
                    break;
                case 180:
                    coord.X += coord2.X;
                    coord.Y += coord2.Y;
                    coord.X -= (rectangleToDraw.Width / 2);
                    coord.Y -= (rectangleToDraw.Height / 2);
                    break;
                case 270:
                    coord.X -= (rectangleToDraw.Height - coord2.Y);
                    coord.Y -= coord2.X;
                    coord.X += (rectangleToDraw.Width / 2);
                    coord.Y += (rectangleToDraw.Height / 2);
                    break;
            }
            

            
            PocketPaintApplication.GetInstance().ToolCurrent.Draw(coord);
        }


        public Point getCenterCoordinateOfGridMain()
        {
            double halfScreenHeight = Window.Current.Bounds.Height / 2.0;
            double halfScreenWidth = Window.Current.Bounds.Width / 2.0;

            TranslateTransform lastTranslateTransform = getLastTranslateTransformation();
            RotateTransform lastRotateTransform = getLastRotateTransformation();

            double offsetX;
            double offsetY;
            if (lastTranslateTransform != null && lastRotateTransform == null)
            {
                offsetX = lastTranslateTransform.X;
                offsetY = lastTranslateTransform.Y;
            }
            else if (lastTranslateTransform == null && lastRotateTransform != null)
            {
                offsetX = 0.0;
                offsetY = 0.0;
            }
            else if (lastTranslateTransform != null && lastRotateTransform != null)
            {
                offsetX = lastTranslateTransform.X;
                offsetY = lastTranslateTransform.Y;
            }
            else
            {
                offsetX = 0.0;
                offsetY = 0.0;
            }

            double marginOffsetX = GridMainSelection.Margin.Left - GridMainSelection.Margin.Right;
            double marginOffsetY = GridMainSelection.Margin.Top - GridMainSelection.Margin.Bottom;

            double coordinateX = offsetX + halfScreenWidth + (marginOffsetX / 2.0);
            double coordinateY = offsetY + halfScreenHeight + (marginOffsetY / 2.0);

            return new Point(coordinateX, coordinateY);
        }

        public void addTransformation(Transform currentTransform)
        {
            for (int i = 0; i < _transformGridMain.Children.Count; i++)
            {
                if (_transformGridMain.Children[i].GetType() == currentTransform.GetType())
                {
                    _transformGridMain.Children.RemoveAt(i);
                }
            }

            _transformGridMain.Children.Add(currentTransform);

            resetAppBarButtonRectangleSelectionControl(true);
            isModifiedRectangleMovement = true;
        }

        public void resetAppBarButtonRectangleSelectionControl(bool isActivated)
        {
            AppBarButton appBarButtonReset = PocketPaintApplication.GetInstance().PaintingAreaView.getAppBarResetButton();
            if (appBarButtonReset != null)
            {
                appBarButtonReset.IsEnabled = isActivated;
            }
        }

        public TranslateTransform getLastTranslateTransformation()
        {
            for (int i = 0; i < _transformGridMain.Children.Count; i++)
            {
                if (_transformGridMain.Children[i].GetType() == typeof(TranslateTransform))
                {
                    TranslateTransform translateTransform = new TranslateTransform();
                    translateTransform.X = ((TranslateTransform)_transformGridMain.Children[i]).X;
                    translateTransform.Y = ((TranslateTransform)_transformGridMain.Children[i]).Y;

                    return translateTransform;
                }
            }

            return null;
        }

        public RotateTransform getLastRotateTransformation()
        {
            for (int i = 0; i < _transformGridMain.Children.Count; i++)
            {
                if (_transformGridMain.Children[i].GetType() == typeof(RotateTransform))
                {
                    RotateTransform rotateTransform = new RotateTransform();
                    rotateTransform.CenterX = ((RotateTransform)_transformGridMain.Children[i]).CenterX;
                    rotateTransform.CenterY = ((RotateTransform)_transformGridMain.Children[i]).CenterY;
                    rotateTransform.Angle = ((RotateTransform)_transformGridMain.Children[i]).Angle;

                    return rotateTransform;
                }
            }

            return null;
        }

        public void SetHeightOfControl(double newHeightRectangleToDraw)
        {
            //TODO 1 is maybe too small?
            if (newHeightRectangleToDraw >= 1)
            {
                GridMainSelection.Height = newHeightRectangleToDraw + (GridMainSelection.Height - rectangleToDraw.Height);
                rectRectangleForMovement.Height = newHeightRectangleToDraw + (rectRectangleForMovement.Height - rectangleToDraw.Height);
                rectRectangleToDraw.Height = newHeightRectangleToDraw;
            }
        }

        public double heightOfRectangleToDraw
        {
            get
            {
                return rectangleToDraw.Height;
            }
            set
            {
                rectangleToDraw.Height = value;
            }
            
        }

        public void SetWidthOfControl(double newWidthRectangleToDraw)
        {
            //TODO 1 is maybe too small?
            if (newWidthRectangleToDraw >= 1)
            {
                GridMainSelection.Width = newWidthRectangleToDraw + (GridMainSelection.Width - rectangleToDraw.Width);
                rectRectangleForMovement.Width = newWidthRectangleToDraw + (rectRectangleForMovement.Width - rectangleToDraw.Width);
                rectRectangleToDraw.Width = newWidthRectangleToDraw;
            }
        }
        public double widthOfRectangleToDraw
        {
            get
            {
                return rectangleToDraw.Width;
            }
            set
            {
                rectangleToDraw.Width = value;
            }
        }

        public Boolean HitRotationArrow(Point point)
        {

            System.Collections.Generic.IEnumerable<UIElement> rotationArrows =
                VisualTreeHelper.FindElementsInHostCoordinates(point, rotationArrowTopRight);
                                        
            if (rotationArrows.Count() != 0)
            {
                return true;
            }

            return false;
        }

        private void ChangeMarginOfGridMainSelection(double leftMargin, double topMargin, double rightMargin, double bottomMargin)
        {        
            GridMainSelection.Margin = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
        }

        public void resetRectangleSelectionControl()
        {
            PocketPaintApplication.GetInstance().PaintingAreaManipulationListener.lastPoint = new Point(0.0, 0.0);

            PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = _rectangleToDrawSize;
            PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = _rectangleToDrawSize;

            _transformGridMain.Children.Clear();
            GridMainSelection.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

            GridMainSelection.Width = _gridMainSize;
            GridMainSelection.Height = _gridMainSize;

            rectRectangleToDraw.Width = _rectangleToDrawSize;
            rectRectangleToDraw.Height = _rectangleToDrawSize;
            rectRectangleForMovement.Width = _rectangleForMovementSize;
            rectRectangleForMovement.Height = _rectangleForMovementSize;

            rectRectangleToDraw.Fill = PocketPaintApplication.GetInstance().PaintData.colorSelected;
            rectRectangleToDraw.Stroke = PocketPaintApplication.GetInstance().PaintData.strokeColorSelected;
            rectRectangleToDraw.StrokeThickness = PocketPaintApplication.GetInstance().PaintData.strokeThickness;
            // strokeLineJoinOfRectangleToDraw = PocketPaintApplication.GetInstance().PaintData.penLineJoinSelected;
            RotateTransform rotate = new RotateTransform();
            var angle = PocketPaintApplication.GetInstance().angularDegreeOfWorkingSpaceRotation;
            rotate.Angle = -angle;
            Point point = new Point(0.5, 0.5);
            PocketPaintApplication.GetInstance().RectangleSelectionControl.RenderTransformOrigin = point;
            PocketPaintApplication.GetInstance().RectangleSelectionControl.RenderTransform = rotate;

            isModifiedRectangleMovement = false;
        }

        public Brush fillOfRectangleToDraw
        {
            get
            {
                return rectangleToDraw.Fill;
            }
            set
            {
                rectangleToDraw.Fill = value;
            }
        }

        public Brush strokeOfRectangleToDraw
        {
            get
            {
                return rectangleToDraw.Stroke;
            }
            set
            {
                rectangleToDraw.Stroke = value;
            }
        }

        public double strokeThicknessOfRectangleToDraw
        {
            get
            {
                return rectangleToDraw.StrokeThickness;
            }
            set
            {
                rectangleToDraw.StrokeThickness = value;
            }
        }

        private void TopLeftGrid_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point deltaTranslation = e.Delta.Translation;
            double deltaTranslationX = deltaTranslation.X * -1.0;
            double deltaTranslationY = deltaTranslation.Y * -1.0;
            double newHeightOfRectangleToDraw = rectangleToDraw.Height + deltaTranslationY;

            if (shouldHeightOfSelectionChanged(newHeightOfRectangleToDraw))
            {
                SetHeightOfControl(newHeightOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left,
                    GridMainSelection.Margin.Top - deltaTranslationY,
                    GridMainSelection.Margin.Right,
                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnHeightValue = newHeightOfRectangleToDraw;
            }

            double newWidthOfRectangleToDraw = rectangleToDraw.Width + deltaTranslationX;
            if (shouldWidthOfSelectionChanged(newWidthOfRectangleToDraw))
            {
                SetWidthOfControl(newWidthOfRectangleToDraw);
                ChangeMarginOfGridMainSelection(GridMainSelection.Margin.Left - deltaTranslationX,
                                    GridMainSelection.Margin.Top,
                                    GridMainSelection.Margin.Right,
                                    GridMainSelection.Margin.Bottom);
                PocketPaintApplication.GetInstance().BarRecEllShape.setBtnWidthValue = newWidthOfRectangleToDraw;
            }
        }
    }
}
