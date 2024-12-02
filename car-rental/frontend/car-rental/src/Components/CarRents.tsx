import {useRents} from "../Context/RentsContext.tsx";
import CarRent from "./CarRent.tsx";

const CarRents = () => {
    const { rents } = useRents();

    if (!Array.isArray(rents)) {
        console.log(rents)
        console.log(typeof rents)
        return <div>No rents available</div>;
    }
    
    return (
        <div className="space-y-4 pl-20">
            {rents.map((rent, index) => (
                <CarRent key={index} rent={rent}/>
            ))}
        </div>
    );
};

export default CarRents;