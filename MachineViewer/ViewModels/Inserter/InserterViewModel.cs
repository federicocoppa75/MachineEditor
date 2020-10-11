using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineViewer.Extensions;
using MachineViewer.Messages.Inserter;
using MachineViewer.Messages.Panel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Inserter
{
    public class InserterViewModel : InserterBaseViewModel
    {
        public double Diameter { get; set; }

        public double Length { get; set; }

        public int LoaderLinkId { get; set; }

        public int DischargerLinkId { get; set; }

        public InserterViewModel() : base()
        {
            Messenger.Default.Register<HookLinkForManageInsertersMessage>(this, OnHookLinkForManageInsertersMessage);
            Messenger.Default.Register<DriveInserterByLinkMessage>(this, OnDriveInserterByLinkMessage);
        }

        private void OnHookLinkForManageInsertersMessage(HookLinkForManageInsertersMessage msg)
        {
            Messenger.Default.Send(new HookLinkByInserterMessage() { InserterId = InserterId, LinkId = LoaderLinkId });
            Messenger.Default.Send(new HookLinkByInserterMessage() { InserterId = InserterId, LinkId = DischargerLinkId });
        }

        private void OnDriveInserterByLinkMessage(DriveInserterByLinkMessage msg)
        {
            if(msg.InserterId == InserterId)
            {
                if(msg.LinkId == LoaderLinkId)
                {
                    LoadInserter(msg.Value);
                }
                else if(msg.LinkId == DischargerLinkId)
                {
                    DischargeInserter(msg.Value);
                }
            }
        }

        private void LoadInserter(bool value)
        {
            if (value)
            {
                var builder = new MeshBuilder();

                builder.AddCylinder(Position, Position + Direction * Length, Diameter / 2.0);

                Children.Add(new MeshGeometryVisual3D()
                {
                    MeshGeometry = builder.ToMesh(),
                    Material = MaterialHelper.CreateMaterial(Color)
                });
            }
        }

        private void DischargeInserter(bool value)
        {
            if (value)
            {
                Children.Clear();

                if (_chainTransform == null) _chainTransform = this.GetChainTansform();

                var t = _chainTransform.Value;
                var p = t.Transform(Position);
                var d = t.Transform(Direction);

                Messenger.Default.Send(new GetPanelTransformMessage()
                {
                    SetData = (b, m) =>
                    {
                        if (b)
                        {
                            var invT = m.Inverse();

                            Messenger.Default.Send(new InsertMessage()
                            {
                                Position = invT.Transform(p),
                                Direction = invT.Transform(d),
                                Color = Color,
                                Length = Length,
                                Diameter = Diameter
                            });
                        }
                    }
                });
            }
        }
    }
}
