namespace WmcSoft.ComponentModel
{
    partial class ComponentWithServiceContainer
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            this.components = new WmcSoft.ComponentModel.NestedContainerWithServiceContainer(this);
            WmcSoft.Net.WakeOnLan wakeOnLan1;
            this.serviceContainerComponent1 = new WmcSoft.ComponentModel.ServiceContainerComponent(this.components);
            wakeOnLan1 = new WmcSoft.Net.WakeOnLan(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.serviceContainerComponent1)).BeginInit();
            // 
            // wakeOnLan1
            // 
            wakeOnLan1.Address = new System.Net.IPAddress(new byte[] {
            ((byte)(127)),
            ((byte)(0)),
            ((byte)(0)),
            ((byte)(2))});
            wakeOnLan1.MacAddress = new System.Net.NetworkInformation.PhysicalAddress(new byte[] {
            ((byte)(92)),
            ((byte)(81)),
            ((byte)(79)),
            ((byte)(100)),
            ((byte)(13)),
            ((byte)(212))});
            ((System.ComponentModel.ISupportInitialize)(this.serviceContainerComponent1)).EndInit();

        }

        #endregion

        private ServiceContainerComponent serviceContainerComponent1;

    }
}
