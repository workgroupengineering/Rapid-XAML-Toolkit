//<auto-generated />
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Microsoft.Xaml.Interactions.Core;

namespace RapidXaml;

public partial class EventToCommand : DependencyObject
{
    public static ICommand GetPreviewKeyDown(DependencyObject obj)
        => (ICommand)obj.GetValue(PreviewKeyDownProperty);

    public static void SetPreviewKeyDown(DependencyObject obj, ICommand value)
        => obj.SetValue(PreviewKeyDownProperty, value);

    public static readonly DependencyProperty PreviewKeyDownProperty =
        DependencyProperty.RegisterAttached("PreviewKeyDown", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, OnPreviewKeyDownChanged));

    private static void OnPreviewKeyDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement uie)
        {
            // Clear any existing behavior(s)
            Interaction.SetBehaviors(uie, null);

            // Add the new one if there is one
            if (e.NewValue is ICommand newCmd)
            {
                var etb = new EventTriggerBehavior { EventName = "PreviewKeyDown" };
                etb.Actions.Add(new InvokeCommandAction { Command = newCmd });

                Interaction.SetBehaviors(uie, new BehaviorCollection { etb });
            }
        }
    }
}
