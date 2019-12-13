﻿using System.Drawing;

namespace TagsCloudVisualization.SourcesTypes
{
    public class Drawable<T>
    {
        public Drawable(T value, Rectangle place)
        {
            Value = value;
            Place = place;
        }

        public Rectangle Place { get; }
        public T Value { get; }
    }
}