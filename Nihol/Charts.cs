using Urho;
using Urho.Actions;


namespace Nihol
{
    public class Charts : Urho.Application
    {
        public Charts(ApplicationOptions options = null) : base(options) { }

        Scene scene;
        Camera camera;
        float actionSpeed = 3;
        int openedPage = 1;
        public string BookNumber;
        protected override void Start()
        {
            
        }
        public void Start2()
        {
            //CreateScene();
            camera = new Camera();
            scene = new Scene();
            var cache = ResourceCache;
            scene.LoadXmlFromCache(cache, $"Scenes/tosura0{BookNumber}.xml");
            camera = scene.GetChild("Cam").GetComponent<Camera>();
            camera.AutoAspectRatio = true;
            camera.Fov = 60.0f;
            //plotNode = scene.CreateChild();
            //var baseNode = plotNode.CreateChild().CreateChild();
            //var plane = baseNode.CreateComponent<StaticModel>();
            //plane.Model = CoreAssets.Models.Plane;

            //var ppp = scene.GetChild("Cam").GetComponent<StaticModel>();
            var page1 = scene.GetChild("Page1");
            var p1h1 = page1.GetChild("H1");
            var p1h2 = page1.GetChild("H2");
            page1.Rotate(new Quaternion(1, 0, 0));
            p1h1.Rotate(new Quaternion(89, 0, 0));
            p1h2.Rotate(new Quaternion(89, 0, 0));
            var page2 = scene.GetChild("Page2");
            var p2v = page2.GetChild("V");
            page2.SetDeepEnabled(true);
            p2v.Rotate(new Quaternion(-89, 0, 0));

            page1.RunActionsAsync(new RotateTo(actionSpeed, 90, 0, 0));
            p2v.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));
            p1h1.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));
            p1h2.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));

            var vp = new Viewport(Context, scene, camera, null);
            Renderer.SetViewport(0, vp);
            vp.SetClearColor(Color.Green);
        }
        public void OpenPage()
        {
            if (openedPage < 1 || openedPage > 3) return;
            var page0 = scene.GetChild("Page" + openedPage);
            var page1 = scene.GetChild("Page" + (openedPage + 1));
            var p1h1 = page1.GetChild("H1");
            var p1h2 = page1.GetChild("H2");
            page1.Rotation = new Quaternion(1, 0, 0);

            p1h1.Rotation = new Quaternion(89, 0, 0);
            p1h2.Rotation = new Quaternion(89, 0, 0);
            var page2 = scene.GetChild("Page" + (openedPage + 2));
            var p2v = page2.GetChild("V");
            page2.Rotation = new Quaternion(0, 0, 0);
            page2.SetDeepEnabled(true);
            p2v.Rotation = new Quaternion(-89, 0, 0);
            page0.RunActionsAsync(new RotateTo(actionSpeed, 180, 0, 0));
            page1.RunActionsAsync(new RotateTo(actionSpeed, 90, 0, 0));
            p2v.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));
            p1h1.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));
            p1h2.RunActionsAsync(new RotateTo(actionSpeed, 0, 0, 0));

            
            openedPage++;
        }

        public void ClosePage()
        {
            if (openedPage < 2 || openedPage > 4) return;
            var page0 = scene.GetChild("Page" + (openedPage-1));
            var page1 = scene.GetChild("Page" + openedPage);
            var p1h1 = page1.GetChild("H1");
            var p1h2 = page1.GetChild("H2");
            var page2 = scene.GetChild("Page" + (openedPage + 1));
            var p1v = page1.GetChild("V");
            page0.Rotation = new Quaternion(90,0,0);
            p1h1.Rotation = new Quaternion();
            p1h2.Rotation = new Quaternion();
            p1v.Rotation = new Quaternion();
            page1.Rotation = new Quaternion();
            page2.SetDeepEnabled(false);

            openedPage--;
        }

        public void ToPage(int pageNumber)
        {
            foreach (var item in scene.Children)
            {
                item.RemoveAllActions();
                foreach (var item2 in item.Children)
                {
                    item2.RemoveAllActions();
                }
            }
            if (pageNumber < 1 || openedPage > 4) return;
            for (int i = 1; i < 6; i++)
            {
                var page = scene.GetChild("Page" + i);
                page.Rotation = new Quaternion();
                page.SetDeepEnabled(false);
                var v = page.GetChild("V");
                if (v != null)
                {
                    v.Rotation = new Quaternion();
                }
                var h1 = page.GetChild("H1");
                if (h1 != null)
                {
                    h1.Rotation = new Quaternion();
                }
                var h2 = page.GetChild("H2");
                if (h2 != null)
                {
                    h2.Rotation = new Quaternion();
                }
            }
            for (int i = 1; i < pageNumber+1; i++)
            {
                var page = scene.GetChild("Page" + i);
                page.Rotation = new Quaternion(180,0,0);
                page.SetDeepEnabled(true);
            }
            var tikkaPage = scene.GetChild("Page" + pageNumber);
            var yotganPage = scene.GetChild("Page" + (pageNumber+1));

            tikkaPage.Rotation = new Quaternion(90, 0, 0);
            yotganPage.SetDeepEnabled(true);
            //var page0 = scene.GetChild("Page" + (openedPage - 1));
            //var page1 = scene.GetChild("Page" + openedPage);
            //var p1h1 = page1.GetChild("H1");
            //var p1h2 = page1.GetChild("H2");
            //var page2 = scene.GetChild("Page" + (openedPage + 1));
            //var p1v = page1.GetChild("V");
            //page0.Rotation = new Quaternion(90, 0, 0);
            //p1h1.Rotation = new Quaternion();
            //p1h2.Rotation = new Quaternion();
            //p1v.Rotation = new Quaternion();
            //page1.Rotation = new Quaternion();
            //page2.SetDeepEnabled(false);

            openedPage = pageNumber;
        }
    }
}
