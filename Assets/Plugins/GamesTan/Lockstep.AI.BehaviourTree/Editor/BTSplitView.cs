using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Lockstep.AI.Editor {
    public class BTSplitView : TwoPaneSplitView {
        public new class UxmlFactory : UxmlFactory<BTSplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}