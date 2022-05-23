namespace Models {
    public record Shape {
        // Size
        public double height;
        public double width;
        // Location
        public double x;
        public double y;
        // Colour
        public Colour fillColour;
        public Colour borderColour;
    }
}