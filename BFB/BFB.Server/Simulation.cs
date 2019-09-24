namespace BFB.Server
{
    public class Simulation
    {

        #region Properties
        
        private readonly object _lock;

        #endregion
        
        #region Constructor
        
        /**
         * Thread safe simulation class that can be ticked to move the simulation forward a single step at a time
         */
        public Simulation()
        {
            _lock = new object();
            //TODO
        }
        
        #endregion

        #region AddEntity
        
        public void AddEntity()
        {
            lock (_lock)
            {
                //TODO
            }
        }

        #endregion
        
        #region RemoveEntity
        
        public void RemoveEntity()
        {
            lock (_lock)
            {
                //TODO
            }
        }
        
        #endregion

        #region SubmitInputUpdates

        public void SubmitUpdates()
        {
            lock (_lock)
            {
                //TODO
            }
        }

        #endregion

        #region GetUpdates

        public void GetUpdates()
        {
            lock (_lock)
            {
                //TODO
            }
        }

        #endregion

        #region Tick
        
        public void Tick()
        {
            lock (_lock)
            {
                //TODO
            }
        }
        
        #endregion
        
    }
}