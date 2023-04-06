//<auto-generated />
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Microsoft.Xaml.Interactions.Core;

namespace RapidXaml;

public partial class EventToCommand : DependencyObject
{
    public static ICommand GetPointerCaptureLost(DependencyObject obj)
        => (ICommand)obj.GetValue(PointerCaptureLostProperty);

    public static void SetPointerCaptureLost(DependencyObject obj, ICommand value)
        => obj.SetValue(PointerCaptureLostProperty, value);

    public static readonly DependencyProperty PointerCaptureLostProperty =
        DependencyProperty.RegisterAttached("PointerCaptureLost", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, OnPointerCaptureLostChanged));

    private static void OnPointerCaptureLostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement uie)
        {
            // Clear any existing behavior(s)
            Interaction.SetBehaviors(uie, null);

            // Add the new one if there is one
            if (e.NewValue is ICommand newCmd)
            {
                var etb = new EventTriggerBehavior { EventName = "PointerCaptureLost" };
                etb.Actions.Add(new InvokeCommandAction { Command = newCmd });

                Interaction.SetBehaviors(uie, new BehaviorCollection { etb });
            }
        }
    }
}
