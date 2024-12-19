import {Rent} from "../Context/RentsContext.tsx";
import {useLocation} from "react-router-dom";
import ConfirmReturnForm from "../Components/ConfirmReturnForm.tsx";

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
            <ConfirmReturnForm rent={rent} />
        </div>
    );
};

export default ConfirmReturnPage;