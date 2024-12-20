import {FormEvent, useEffect, useState} from "react";
import {FaSearch} from "react-icons/fa";
import {useFilters} from "../Context/FiltersContext.tsx";
import {rentStatus, useRents} from "../Context/RentsContext.tsx";

const FiltersBar = () => {
    const {filters, setFilters} = useFilters();
    const {fetchRents} = useRents();

    const [selectedRentStatus, setSelectedRentStatus] = useState<rentStatus>(filters.selectedRentStatus);

    useEffect(() => {
        // Fetch the rent list data
        fetchRents();
    }, [filters]);
    
    const handleSubmit = (e : FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        setFilters({
            selectedRentStatus: selectedRentStatus,
        })
    };

    const formatStatus = (status: rentStatus): string => {
        const mapping: Record<rentStatus, string> = {
            [rentStatus.Active]: "Active",
            [rentStatus.ReadyToReturn]: "Ready To Return",
            [rentStatus.Returned]: "Returned",
        };
        return mapping[status] || status;
    };

    return (
        <>
            <form
                className="flex flex-wrap items-center p-4 bg-white rounded-lg shadow-md w-full max-w-screen-lg mx-auto"
                onSubmit={handleSubmit}
            >
                <div className="flex w-full justify-between flex-col md:flex-row">
                    {/* Status Dropdown */}
                    <select
                        value={selectedRentStatus}
                        onChange={(e) => setSelectedRentStatus(e.target.value as rentStatus)}
                        className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                    >
                        {Object.values(rentStatus).map((status) => (
                            <option key={status} value={status}>
                                {formatStatus(status)}
                            </option>
                        ))}
                    </select>

                    <button
                        type="submit"
                        className="bg-black text-white rounded-lg p-2 mt-2 md:mt-0 md:ml-2 w-full md:w-auto"
                    >
                        <FaSearch className="inline text-lg mb-1"/> Apply Filters
                    </button>
                </div>
            </form>
        </>
    );
};

export default FiltersBar;