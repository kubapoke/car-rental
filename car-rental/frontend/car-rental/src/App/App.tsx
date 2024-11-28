import '../Styles/index.css'
import {
    Route,
    createBrowserRouter,
    createRoutesFromElements,
    RouterProvider
} from 'react-router-dom';
import NewUserForm from "../Pages/LogInPage.tsx"
import MainLayout from "../Layout/MainLayout.tsx";
import HomePage from "../Pages/HomePage.tsx";
import {AuthProvider} from "../Context/AuthContext.tsx";

function App() {
    const router = createBrowserRouter(
        createRoutesFromElements(
            <Route path="/" element={<MainLayout/>}>
                <Route index element={<HomePage/>}/>
                <Route path='/log-in-page' element={<NewUserForm/>}/>
            </Route>
        )
    );

  return (

      <AuthProvider>
          <RouterProvider router={router}/>
      </AuthProvider>
  )
}

export default App
