using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.Util
{
    public class Rotation3DBehavior : Behavior<ModelUIElement3D>
    {
        Window parrent = Application.Current.MainWindow;

        private Point mouseStartPosition;
        private double startAngle;

        private Transform3DGroup transform = new Transform3DGroup();
        private RotateTransform3D rotateTransform = new RotateTransform3D();
        private AxisAngleRotation3D rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);

        protected override void OnAttached()
        {
            rotateTransform.Rotation = rotation;
            transform.Children.Add(rotateTransform);

            /*ScaleTransform3D scaleTransform = new ScaleTransform3D();
            Binding binding = new Binding();
            binding.Path = new PropertyPath("ModelScale");
            BindingOperations.SetBinding(scaleTransform, ScaleTransform3D.ScaleXProperty, binding);
            BindingOperations.SetBinding(scaleTransform, ScaleTransform3D.ScaleYProperty, binding);
            BindingOperations.SetBinding(scaleTransform, ScaleTransform3D.ScaleZProperty, binding);
            transform.Children.Add(scaleTransform);*/

            AssociatedObject.Transform = transform;

            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseMove += OnMouseMove;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.MouseMove -= OnMouseMove;
        }

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AssociatedObject.IsMouseCaptured)
            {
                Vector diff = e.GetPosition(parrent) - mouseStartPosition;
                rotation.Angle = startAngle + diff.X;
            }
        }

        private void OnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AssociatedObject.ReleaseMouseCapture();
        }

        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            startAngle = rotation.Angle;
            mouseStartPosition = e.GetPosition(parrent);
            AssociatedObject.CaptureMouse();
        }        
    }
}
