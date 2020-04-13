using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MatrixTransformations.Control
{
    public delegate void KeyboardStateChangedHandler(KeyboardState newState);

    public class KeyboardState
    {
        private readonly Dictionary<Keys, bool> keyStates;

        public event KeyboardStateChangedHandler OnKeyboardStateChanged;

        public KeyboardState()
        {
            this.keyStates = new Dictionary<Keys, bool>();
        }

        public void SetIsKeyPressed(Keys key, bool pressed)
        {
            if (this.keyStates.ContainsKey(key)) { this.keyStates[key] = pressed; }
            else { this.keyStates.Add(key, pressed); }

            OnKeyboardStateChanged?.Invoke(this);
        }

        public bool IsKeyPressed(Keys key)
        {
            return this.keyStates.ContainsKey(key) && this.keyStates[key];
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<Keys, bool> keyValuePair in this.keyStates) { stringBuilder.Append(keyValuePair.Key + ": " + keyValuePair.Value + '\n'); }

            return stringBuilder.ToString();
        }
    }
}