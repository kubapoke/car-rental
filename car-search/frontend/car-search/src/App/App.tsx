import '../index.css'
import {
    Route,
    createBrowserRouter,
    createRoutesFromElements,
    RouterProvider
} from 'react-router-dom';
import MainLayout from "../Layouts/MainLayout.tsx";
import HomePage from "../Pages/HomePage.tsx";
import NotFoundPage from "../Pages/NotFoundPage.tsx";

function App() {
  const router = createBrowserRouter(
      createRoutesFromElements(
          <Route path="/" element={<MainLayout/>}>
              <Route index element={<HomePage/>}/>
              <Route path="*" element={<NotFoundPage/>}/>
          </Route>
      )
  );
  
  return <RouterProvider router={router}/>
}

export default App
