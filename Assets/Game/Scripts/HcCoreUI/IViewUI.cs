namespace GeneralUI.Components
{

    public interface IViewUI
    {
        public float Delay { get; }
        public float Duration { get; }
        public bool IgnoreDuration { get; }

        public void Show();

        public void Hide();

        public void Rewind();
    }

    public static class ViewUIExtension
    {
        public static void Show(this IViewUI[] views)
        {
            foreach (var view in views)
                view.Show();
        }

        public static void Hide(this IViewUI[] views)
        {
            foreach (var view in views)
                view.Hide();
        }

        public static void Rewind(this IViewUI[] views)
        {
            foreach (var view in views)
                view.Rewind();
        }
    }
}