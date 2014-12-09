using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Tripper.MeshMisc;
using Tripper.Navigation;
using Tripper.RecastManaged.Detour;
using Vector3 = Tripper.Tools.Math.Vector3;

namespace GarrisonBuddy
{
    public class NavigationGaB : MeshNavigator
    {
        private static readonly Stopwatch StuckWatch = new Stopwatch();
        private readonly WaitTimer _waitTimer1 = new WaitTimer(TimeSpan.FromSeconds(1.0));
        private readonly WaitTimer _waitTimer2 = WaitTimer.FiveSeconds;
        private WoWPoint _currentDestination;
        private MeshMovePath _currentMovePath2;
        private StuckHandler _stuckHandlerGaB;
        public override float PathPrecision { get; set; }

        public override void OnRemoveAsCurrent()
        {
            base.OnRemoveAsCurrent();
        }

        public override float? PathDistance(WoWPoint @from, WoWPoint to, float maxDistance = (float) 3.402823E+38)
        {
            return base.PathDistance(@from, to, maxDistance);
        }

        public override MoveResult MovePath(MeshMovePath path)
        {
            MoveResult res = base.MovePath(path);
            return res;
        }

        public override void OnSetAsCurrent()
        {
            base.OnSetAsCurrent();
            _stuckHandlerGaB = StuckHandler;
            _stuckHandlerGaB.Reset();
        }

        public override bool CanNavigateWithin(WoWPoint @from, WoWPoint to, float distanceTolerancy)
        {
            return base.CanNavigateWithin(@from, to, distanceTolerancy);
        }

        public override bool CanNavigateFully(WoWPoint @from, WoWPoint to)
        {
            return base.CanNavigateFully(@from, to);
        }

        private WoWPoint GetDestination()
        {
            return _currentDestination;
        }

        public override MoveResult MoveTo(WoWPoint location)
        {
            _currentDestination = location;
            if (location == WoWPoint.Zero)
                return MoveResult.Failed;

            WoWUnit activeMover = WoWMovement.ActiveMover;
            if (activeMover == null)
                return MoveResult.Failed;

            WoWPoint MoverLocation = activeMover.Location;

            if (_stuckHandlerGaB.IsStuck())
            {
                GarrisonBuddy.Diagnostic("Is stuck! ");
                _stuckHandlerGaB.Unstick();
                return MoveResult.UnstuckAttempt;
            }
            if (MoverLocation.Distance2DSqr(Coroutine.Dijkstra.ClosestToNodes(location)) < 1f)
            {
                Clear();
                _stuckHandlerGaB.Reset();
                StuckWatch.Reset();
                return MoveResult.ReachedDestination;
            }
            if (MoverLocation.Distance2DSqr(Coroutine.Dijkstra.ClosestToNodes(location)) < 3f)
            {
                Navigator.PlayerMover.MoveTowards(location);
                return MoveResult.Moved;
            }
            if (Mount.ShouldMount(location))
            {
                Mount.StateMount(GetDestination);
            }
            if (_waitTimer1.IsFinished)
            {
                WoWGameObject woWgameObject =
                    ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                        .FirstOrDefault(param0 =>
                        {
                            if (param0.SubType == WoWGameObjectType.Door && ((WoWDoor) param0.SubObj).IsClosed &&
                                (!param0.Locked && param0.WithinInteractRange) && param0.CanUse())
                                return param0.CanUseNow();
                            return false;
                        });
                if (woWgameObject != null)
                {
                    woWgameObject.Interact();
                }
                _waitTimer1.Reset();
            }
            bool flag = false;
            if (_currentMovePath2 == null || _currentMovePath2.Path.End.DistanceSqr(location) > 9.0f)
            {
                flag = true;
            }

            else if (_waitTimer2.IsFinished && Unnamed2(_currentMovePath2, MoverLocation))
            {
                WoWMovement.MoveStop();
                flag = true;
                _waitTimer2.Reset();
            }
            if (!flag)
            {
                _stuckHandlerGaB.Reset();
                return MovePath(_currentMovePath2);
            }
            WoWPoint startFp;
            WoWPoint endFp;
            if (MoverLocation.DistanceSqr(location) > 160000.0 &&
                FlightPaths.ShouldTakeFlightpath(MoverLocation, location, activeMover.MovementInfo.RunSpeed) &&
                FlightPaths.SetFlightPathUsage(MoverLocation, location, out startFp, out endFp))
            {
                _stuckHandlerGaB.Reset();
                return MoveResult.PathGenerated;
            }
            PathFindResult path = FindPath(MoverLocation, location);
            if (!path.Succeeded)
            {
                _stuckHandlerGaB.Reset();
                return MoveResult.PathGenerationFailed;
            }
            _currentMovePath2 = new MeshMovePath(path);
            _stuckHandlerGaB.Reset();
            return MoveResult.PathGenerated;
        }

        private bool Unnamed2(MeshMovePath param0, Vector3 param1)
        {
            if ((WoWMovement.ActiveMover ?? StyxWoW.Me).IsFalling || param0.Index <= 0 ||
                (param0.Index >= param0.Path.Points.Length))
                return true;
            return false;
        }


        private PathFindResult FindPathInner(PathFindResult pathFindResult)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Vector3[] points = Coroutine.Dijkstra.GetPath2(pathFindResult.Start, pathFindResult.End);
            stopWatch.Stop();
            var abilities = new AbilityFlags[points.Count()];
            var PolygonReferences = new PolygonReference[points.Count()];
            var straightpaths = new StraightPathFlags[points.Count()];
            var AreaTypes = new AreaType[points.Count()];


            for (int index = 0; index < points.Length; index++)
            {
                straightpaths[index] = StraightPathFlags.None;
                PolygonReferences[index] = new PolygonReference();
                abilities[index] = AbilityFlags.Run;
                AreaTypes[index] = AreaType.Ground;
            }


            return new PathFindResult
            {
                AbilityFlags = abilities,
                Aborted = false,
                Status = Status.Success,
                Flags = straightpaths,
                Points = points,
                Polygons = PolygonReferences,
                PolyTypes = AreaTypes,
                Start = pathFindResult.Start,
                End = pathFindResult.End,
                Elapsed = stopWatch.Elapsed,
                IsPartialPath = false
            };
        }

        private new PathFindResult FindPath(WoWPoint start, WoWPoint end)
        {
            var obj = new PathFindResult {Start = start, End = end};
            if (TreeRoot.State == TreeRootState.Stopping)
            {
                return new PathFindResult
                {
                    AbilityFlags = new AbilityFlags[0],
                    Aborted = true,
                    Status = Status.Failure,
                    Flags = new StraightPathFlags[0],
                    Points = new Vector3[0],
                    Polygons = new PolygonReference[0],
                    PolyTypes = new AreaType[0],
                    Start = obj.Start,
                    End = obj.End
                };
            }
            Task<PathFindResult> task = Task<PathFindResult>.Factory.StartNew(() => FindPathInner(obj));
            DateTime startedAt = DateTime.Now;
            try
            {
                //while it is not done with timeout 
                while ((DateTime.Now - startedAt).TotalMilliseconds < 1000/TreeRoot.TicksPerSecond || task.IsCompleted)
                {
                    try
                    {
                        StyxWoW.Memory.ReleaseFrame();
                        ObjectManager.Update();
                        WoWMovement.Pulse();
                        StyxWoW.ResetAfk();
                        StyxWoW.Memory.AcquireFrame();
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteException(ex);
                    }
                }
                return task.Result;
            }
            finally
            {
                task.Dispose();
            }
        }

        public override WoWPoint[] GeneratePath(WoWPoint @from, WoWPoint to)
        {
            Logging.Write("TEST GENERATE PATH");
            return Coroutine.Dijkstra.GetPathWoW(@from, to);
        }

        public override bool AtLocation(WoWPoint point1, WoWPoint point2)
        {
            Logging.Write("TEST AtLocation");
            return Coroutine.Dijkstra.ClosestToNodes(point1).Distance(Coroutine.Dijkstra.ClosestToNodes(point2)) < 3;
        }
    }
}