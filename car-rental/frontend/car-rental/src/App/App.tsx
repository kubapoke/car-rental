import '../Styles/index.css';
import {createBrowserRouter, createRoutesFromElements, RouterProvider, Navigate, Route} from 'react-router-dom';
import { AuthProvider, useAuth } from "../Context/AuthContext.tsx";  // Zaimportuj AuthProvider
import LogInPage from "../Pages/LogInPage.tsx";
import CockpitPage from "../Pages/CockpitPage.tsx";
import LoggedInLayout from "../Layouts/LoggedInLayout.tsx";
import {FiltersProvider} from "../Context/FiltersContext.tsx";

function App() {
    return (
        <AuthProvider>  {/* Umieść AuthProvider wokół całej aplikacji */}
            <FiltersProvider>
                <MainRouter />
            </FiltersProvider>
        </AuthProvider>
    );
}

function MainRouter() {
    const { isLoggedIn } = useAuth();

    const router = createBrowserRouter(
        createRoutesFromElements(
            <>
                <Route index element={<Navigate to="/log-in" replace />} />
                <Route path="*" element={<Navigate to={isLoggedIn ? "/logged-in/cockpit" : "/log-in"} replace />} />
                <Route path="/log-in" element={<LogInPage />} />
                <Route path="/logged-in" element={<LoggedInLayout />}>
                    <Route path="cockpit" element={<CockpitPage />} />
                </Route>
            </>
        )
    );

    return <RouterProvider router={router} />;
}

export default App
