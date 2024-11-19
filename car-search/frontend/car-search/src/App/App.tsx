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
import OfferPage from "../Pages/OfferPage.tsx";
import NewUserForm from "../Pages/NewUserForm.tsx";
import {FiltersProvider} from "../Context/FiltersContext.tsx";
import {OffersProvider} from "../Context/OffersContext.tsx";


function App() {
    const router = createBrowserRouter(
      createRoutesFromElements(
          <Route path="/" element={<MainLayout/>}>
              <Route index element={<HomePage/>}/>
              <Route path='/offers' element={<BrowseOffersPage/>}/>
              <Route path='/new-user-form' element={<NewUserForm/>}/>
              <Route path='/offers/:id' element={<OfferPage/>}/>
              <Route path="*" element={<NotFoundPage/>}/>
          </Route>
      )
    );
  
  return(
      <FiltersProvider>
          <OffersProvider>
              <RouterProvider router={router}/>
          </OffersProvider>
      </FiltersProvider>
  );
}

export default App
