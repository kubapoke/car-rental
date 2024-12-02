import React, { createContext, useContext, useState } from 'react';
import {rentStatus, useFilters} from "./FiltersContext.tsx";


export interface Rent {
    brand: string,
    model: string,
    startDate: string,
    endDate: string,
    status: rentStatus
}


interface RentsContextProps {
    rents: Rent[];
    setRents: React.Dispatch<React.SetStateAction<Rent[]>>;
    fetchRents: () => Promise<void>;
}

const RentsContext = createContext<RentsContextProps | undefined>(undefined);

export const RentsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [rents, setRents] = useState<Rent[]>([]);
    const { filters } = useFilters();
    
    // fetch offers based on filters
    const fetchRents = async () => {
        console.log("fetching data")

        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };

        const query = new URLSearchParams({
            rentStatus: filters.selectedRentStatus,
        }).toString();

        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/get-rents?${query}`;
        try {
            const response = await fetch(apiUrl, {method: 'GET', headers: headers});
            const data: Rent[] = await response.json();

            console.log(typeof data)

            if (Array.isArray(data)) {
                setRents(data);
            } else {
                console.error('Expected an array but received:', data);
                setRents([]);
            }

            console.log('Rentals:', data);
        } catch (error) {
            console.error('Error fetching car list:', error);
            setRents([]);
        }
    };

    return (
        <RentsContext.Provider value={{ rents, setRents, fetchRents }}>
            {children}
        </RentsContext.Provider>
    );
};

export const useRents = (): RentsContextProps => {
    const context = useContext(RentsContext);
    if (!context) {
        throw new Error('useRents must be used within an RentsProvider');
    }
    return context;
};
