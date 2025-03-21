namespace Workshop_api_greenfield.Domain.Entities
{
    /// <summary>
    /// Represents features available in rooms that can be reserved.
    /// </summary>
    public enum RoomFeature
    {
        /// <summary>
        /// Room has a projector.
        /// </summary>
        Projector,

        /// <summary>
        /// Room has a whiteboard.
        /// </summary>
        Whiteboard,

        /// <summary>
        /// Room has video conferencing equipment.
        /// </summary>
        VideoConference,

        /// <summary>
        /// Room has a TV screen.
        /// </summary>
        TVScreen,

        /// <summary>
        /// Room has a computer.
        /// </summary>
        Computer,

        /// <summary>
        /// Room has air conditioning.
        /// </summary>
        AirConditioning,

        /// <summary>
        /// Room is wheelchair accessible.
        /// </summary>
        WheelchairAccessible
    }
} 