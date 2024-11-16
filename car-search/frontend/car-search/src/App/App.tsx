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
import BrowseOffersPage from "../Pages/BrowseOffersPage.tsx";
import OfferPage, {offerLoader} from "../Pages/OfferPage.tsx";
import {toast} from 'react-toastify';
import NewUserForm from "../Pages/NewUserForm.tsx";


function App() {
    
    const rentCar = (id: string) => {
        console.log('Request for car id: ', id);
        toast.success("Request for car rental sent successfully!");
    }
    
    const router = createBrowserRouter(
      createRoutesFromElements(
          <Route path="/" element={<MainLayout/>}>
              <Route index element={<HomePage/>}/>
              <Route path='/offers' element={<BrowseOffersPage/>}/>
              <Route path='/new-user-form' element={<NewUserForm/>}/>
              <Route path='/offers/:id' element={<OfferPage rentCar={rentCar}/>} loader={offerLoader}/>
              <Route path="*" element={<NotFoundPage/>}/>
          </Route>
      )
    );
  
  return <RouterProvider router={router}/>
}

export default App
