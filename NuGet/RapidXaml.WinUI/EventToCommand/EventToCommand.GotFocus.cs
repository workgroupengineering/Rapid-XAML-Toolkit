//<auto-generated />
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Microsoft.Xaml.Interactions.Core;

namespace RapidXaml;

public partial class EventToCommand : DependencyObject
{
    public static ICommand GetGotFocus(DependencyObject obj)
        => (ICommand)obj.GetValue(GotFocusProperty);

    public static void SetGotFocus(DependencyObject obj, ICommand value)
        => obj.SetValue(GotFocusProperty, value);

    public static readonly DependencyProperty GotFocusProperty =
        DependencyProperty.RegisterAttached("GotFocus", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, OnGotFocusChanged));

    private static void OnGotFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement uie)
        {
            // Clear any existing behavior(s)
            Interaction.SetBehaviors(uie, null);

            // Add the new one if there is one
            if (e.NewValue is ICommand newCmd)
            {
                var etb = new EventTriggerBehavior { EventName = "GotFocus" };
                etb.Actions.Add(new InvokeCommandAction { Command = newCmd });

                Interaction.SetBehaviors(uie, new BehaviorCollection { etb });
            }
        }
    }
}
