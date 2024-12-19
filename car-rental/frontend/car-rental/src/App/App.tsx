import '../Styles/index.css';
import {createBrowserRouter, createRoutesFromElements, RouterProvider, Navigate, Route} from 'react-router-dom';
import { AuthProvider, useAuth } from "../Context/AuthContext.tsx";  // Zaimportuj AuthProvider
import LogInPage from "../Pages/LogInPage.tsx";
import CockpitPage from "../Pages/CockpitPage.tsx";
import LoggedInLayout from "../Layouts/LoggedInLayout.tsx";
import {FiltersProvider} from "../Context/FiltersContext.tsx";
import {RentsProvider} from "../Context/RentsContext.tsx";
import ConfirmReturnPage from "../Pages/ConfirmReturnPage.tsx";

function App() {
    return (
        <AuthProvider>  {/* Umieść AuthProvider wokół całej aplikacji */}
            <FiltersProvider>
                <RentsProvider>
                    <MainRouter />
                </RentsProvider>
            </FiltersProvider>
        </AuthProvider>
    );
}

function MainRouter() {
    const { isLoggedIn } = useAuth();

    const router = createBrowserRouter(
        createRoutesFromElements(
            <>
                <Route index element={<Navigate to={isLoggedIn ? "/logged-in/cockpit" : "/log-in"} replace />} />
                <Route path="log-in" element={isLoggedIn ? <Navigate to="/logged-in/cockpit" replace /> : <LogInPage />} />
                <Route path="logged-in" element={isLoggedIn ? <LoggedInLayout /> : <Navigate to="/log-in" replace />}>
                    <Route path="cockpit" element={isLoggedIn ? <CockpitPage /> : <Navigate to="/log-in" replace />} />
                    <Route path="confirm-return/:id" element={isLoggedIn ? <ConfirmReturnPage/> : <Navigate to="/log-in" replace />}/>
                </Route>
            </>
        )
    );

    return <RouterProvider router={router} />;
}

export default App
