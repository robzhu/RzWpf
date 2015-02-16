using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RzWpf.AttachedProperties
{
    public static class TextBoxBehavior
    {
        #region Dependency Property

        public static readonly DependencyProperty IsOnEscapeKeyClear =
            DependencyProperty.RegisterAttached("IsOnEscapeKeyClear", typeof(Boolean), typeof(TextBoxBehavior),
            new UIPropertyMetadata(false, TextBox_OnEscapeKey));

        #endregion

        #region Get and Set Accessors Static Methods


        public static Boolean GetIsOnEscapeKeyClear(DependencyObject DObj)
        {
            return (Boolean)DObj.GetValue(IsOnEscapeKeyClear);
        }

        public static void SetIsOnEscapeKeyClear(DependencyObject DObj, Boolean Value)
        {
            DObj.SetValue(IsOnEscapeKeyClear, (Boolean)Value);
        }


        #endregion

        #region Dependency Property

        public static void TextBox_OnEscapeKey(Object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox txtObj = (TextBox)sender;

            if (txtObj != null)
            {
                if ((Boolean)e.NewValue)
                {
                    txtObj.KeyDown += (s, o) =>
                    {

                        if (o.Key == System.Windows.Input.Key.Escape)
                        {
                            if (txtObj.Text != String.Empty)
                            {
                                txtObj.Text = String.Empty;
                            }
                        }
                    };
                }
            }
           
        }

        #endregion
    }
}
