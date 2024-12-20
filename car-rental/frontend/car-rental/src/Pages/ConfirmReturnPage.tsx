import { Rent } from "../Context/RentsContext.tsx";
import { useLocation } from "react-router-dom";
import ConfirmReturnForm from "../Components/ConfirmReturnForm.tsx";

const ConfirmReturnPage = () => {
    const location = useLocation();
    const { rent } = location.state as { rent: Rent };

    return (
        <div>
            <ConfirmReturnForm rent={rent} />
        </div>
    );
};

export default ConfirmReturnPage;
