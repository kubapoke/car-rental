import '../index.css'
import {
    Route,
    createBrowserRouter,
    createRoutesFromElements,
    RouterProvider
} from 'react-router-dom';
import MainLayout from "../Layouts/MainLayout.tsx";

function App() {
  const router = createBrowserRouter(
      createRoutesFromElements(
          <Route path="/" element={<MainLayout/>}>
              
          </Route>
      )
  );
  
  return <RouterProvider router={router}/>
}

export default App
