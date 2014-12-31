using System.ComponentModel;

namespace RzWpf
{
    /// <summary>
    /// Interface for an element that is meant to be hosted in a canvas.
    /// </summary>
    public interface ICanvasElement : INotifyPropertyChanged
    {
        /// <summary>
        /// The X position of the image Origin on the host canvas.
        /// </summary>
        double CanvasX { get; set; }

        /// <summary>
        /// The Y position of the image Origin on the host canvas.
        /// </summary>
        double CanvasY { get; set; }

        /// <summary>
        /// The Z Index of this image on the host canvas.
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// This value is meant to be used by a View style to apply a shader/effect based on certain keys.
        /// </summary>
        string EffectId { get; set; }

        bool Visible { get; set; }
    }
}
