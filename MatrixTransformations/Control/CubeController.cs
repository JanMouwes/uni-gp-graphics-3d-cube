using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MatrixTransformations.Control
{
    public class CubeController
    {
        public CubeController()
        {
            this.TranslationVector = new Vector();
        }

        public void Update(KeyboardState newState)
        {
            UpdateTranslation(newState);
            UpdateRotation(newState);
            UpdateScale(newState);
        }

        public float RotateX { get; set; }
        public float RotateY { get; set; }
        public float RotateZ { get; set; }

        public float Scale { get; set; } = 1;
        public Vector TranslationVector { get; }

        public Matrix ScaleMatrix => Matrix.ScaleMatrix(Scale);

        public Matrix RotateMatrix => Matrix.RotateMatrix(RotateX, RotateY, RotateZ);

        public Matrix TranslateMatrix => Matrix.TranslateMatrix(TranslationVector);

        private void UpdateTranslation(KeyboardState input)
        {
            const float stepSize = 0.1f;

            if (input.IsKeyPressed(Keys.Left)) { TranslationVector.x -= stepSize; }

            if (input.IsKeyPressed(Keys.Right)) { TranslationVector.x += stepSize; }

            if (input.IsKeyPressed(Keys.Down)) { TranslationVector.y -= stepSize; }

            if (input.IsKeyPressed(Keys.Up)) { TranslationVector.y += stepSize; }

            if (input.IsKeyPressed(Keys.PageDown)) { TranslationVector.z -= stepSize; }

            if (input.IsKeyPressed(Keys.PageUp)) { TranslationVector.z += stepSize; }

            if (input.IsKeyPressed(Keys.OemMinus)) { TranslationVector.z -= stepSize; }

            if (input.IsKeyPressed(Keys.Oemplus)) { TranslationVector.z += stepSize; }
        }

        private void UpdateRotation(KeyboardState input)
        {
            const float stepSizeBase = 0.1f;

            bool isShiftPressed = input.IsKeyPressed(Keys.ShiftKey);
            float stepSize = isShiftPressed ? -stepSizeBase : stepSizeBase;

            if (input.IsKeyPressed(Keys.X)) { RotateX += stepSize; }

            if (input.IsKeyPressed(Keys.Y)) { RotateY += stepSize; }

            if (input.IsKeyPressed(Keys.Z)) { RotateZ += stepSize; }
        }

        private void UpdateScale(KeyboardState input)
        {
            const float stepSizeBase = 0.1f;
            bool isShiftPressed = input.IsKeyPressed(Keys.ShiftKey);
            float stepSize = isShiftPressed ? -stepSizeBase : stepSizeBase;

            if (input.IsKeyPressed(Keys.S)) { Scale += stepSize; }
        }

        public IEnumerable<Vector> TransformVectors(IEnumerable<Vector> input)
        {
            foreach (Vector vector in input) { yield return TranslateMatrix * RotateMatrix * ScaleMatrix * vector; }
        }

        public static CubeController Default => new CubeController() { Scale = 1};

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("RotateX: " + RotateX + '\n');
            stringBuilder.Append("RotateY: " + RotateY + '\n');
            stringBuilder.Append("RotateZ: " + RotateZ + '\n');

            stringBuilder.Append("Scale: " + Scale + '\n');

            stringBuilder.Append("Translation: " + TranslationVector + '\n');

            return stringBuilder.ToString();
        }
    }
}