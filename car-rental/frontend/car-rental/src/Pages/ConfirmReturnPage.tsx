import {Rent} from "../Context/RentsContext.tsx";
import {useLocation} from "react-router-dom";

const ConfirmReturnPage = () => {
    const location = useLocation();
    const { rent } = location.state as { rent: Rent };

    return (
        <div>
            <h1>Confirm Return</h1>
            <div>
                <h3>{rent.car.model.brand.name} {rent.car.model.name}</h3>
                <p><strong>Status:</strong> {rent.status}</p>
                <p><strong>Start Date:</strong> {rent.rentStart}</p>
                <p><strong>End Date:</strong> {rent.rentEnd || "N/A"}</p>
            </div>
        </div>
    );
};

export default ConfirmReturnPage;