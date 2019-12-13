﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Providers.Layouter.Interfaces;
using TagsCloudVisualization.Providers.Layouter.Spirals;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.SourcesTypes;

namespace TagsCloudVisualization.Providers.Layouter
{
    public class DrawableCloudLayouter : IDrawableProvider<string>
    {
        private readonly SpiralFactory factory;
        public readonly List<Rectangle> Rectangles;
        private Point center;
        private ISpiral spiralPointer;

        public DrawableCloudLayouter(SpiralFactory factory)
        {
            Rectangles = new List<Rectangle>();
            this.factory = factory;
        }

        public IEnumerable<Drawable<string>> GetDrawableObjects(IEnumerable<Sizable<string>> objects,
            LayouterSettings settings)
        {
            center = settings.Center;
            spiralPointer = factory.Create(settings);

            return objects.Select(sizable => new Drawable<string>(sizable.Value, PutNextRectangle(sizable.DrawSize)));
        }

        private Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty || rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle does not exist");

            var rect = new Rectangle(spiralPointer.GetSpiralCurrent(), rectangleSize);
            while (Rectangles.Any(currentR => currentR.IntersectsWith(rect)))
            {
                var currentPoint = spiralPointer.GetSpiralNext();
                rect.X = currentPoint.X;
                rect.Y = currentPoint.Y;
            }

            rect = GetRectangleMovedToCenter(rect);

            Rectangles.Add(rect);
            return rect;
        }

        private Rectangle GetRectangleMovedToCenter(Rectangle rect)
        {
            while (TryMoveToCenter(rect, out rect)) ;
            return rect;
        }

        private bool TryMoveToCenter(Rectangle rect, out Rectangle rectOut)
        {
            {
                if (rect.X == center.X && rect.Y == center.Y)
                {
                    rectOut = rect;
                    return false;
                }

                var dx = center.X - rect.X;
                var dy = center.Y - rect.Y;

                rectOut = rect;
                var canMoveX = dx != 0 &&
                               (dx > 0
                                   ? TryMove(rect, 1, 0, out rectOut)
                                   : TryMove(rect, -1, 0, out rectOut));
                var canMoveY = dy != 0 &&
                               (dy > 0
                                   ? TryMove(rectOut, 0, 1, out rectOut)
                                   : TryMove(rectOut, 0, -1, out rectOut));
                return canMoveX || canMoveY;
            }
        }

        private bool TryMove(Rectangle rect, int dx, int dy, out Rectangle rectOut)
        {
            {
                rect.X += dx;
                rect.Y += dy;
                rectOut = rect;
                if (!Rectangles.Any(r => r.IntersectsWith(rect)))
                    return true;

                rectOut.X -= dx;
                rectOut.Y -= dy;
                return false;
            }
        }
    }
}