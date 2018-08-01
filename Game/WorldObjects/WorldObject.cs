﻿using ClassicUO.Game.Map;
using ClassicUO.Game.Renderer.Views;
using Microsoft.Xna.Framework;

namespace ClassicUO.Game.WorldObjects
{
    public abstract class WorldObject //: IDisposable
    {
        private Tile _tile;
        private View _viewObject;

        protected WorldObject(in Facet map)
        {
            Map = map;
        }

        /// <summary>
        ///     Multiply X * 22, Y * 22.
        ///     Z is used to do Depth testing
        /// </summary>
        public Vector3 ScreenPosition
        {
            get
            {
                float screenX = (Position.X - Position.Y) * 22;
                float screenY = (Position.X + Position.Y) * 22 /*- Position.Z * 4*/;

                return new Vector3(screenX, screenY, 0);
            }
        }


        public virtual Position Position { get; set; } = Position.Invalid;
        public virtual Hue Hue { get; set; }
        public virtual Graphic Graphic { get; set; }

        public View ViewObject => _viewObject ?? (_viewObject = CreateView());

        public sbyte AnimIndex { get; set; }

        public Tile Tile
        {
            get => _tile;
            set
            {
                if (_tile != value)
                {
                    _tile?.RemoveWorldObject(this);

                    _tile = value;

                    if (_tile != null)
                    {
                        _tile.AddWorldObject(this);
                    }
                    else
                    {
                        if (this != World.Player && !IsDisposed)
                            Dispose();
                    }
                }
            }
        }


        public Facet Map { get; set; }

        public bool IsDisposed { get; private set; }

        protected virtual View CreateView()
        {
            return null;
        }

        protected void DisposeView()
        {
            if (_viewObject != null)
            {
                _viewObject.Dispose();
                _viewObject = null;
            }
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            DisposeView();
            //if (Deferred != null)
            //    Deferred.Tile = null;
            //Deferred = null;
            Tile = null;
        }

        //public DeferredEntity Deferred { get; set; }


        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(in bool disposing)
        //{
        //    if (disposing)
        //    {
        //        DisposeView();
        //    }
        //}

        //~WorldObject()
        //{
        //    Dispose();
        //}
    }
}