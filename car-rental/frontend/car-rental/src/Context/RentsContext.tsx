import React, { createContext, useContext, useState } from 'react';
import {rentStatus, useFilters} from "./FiltersContext.tsx";


export interface Rent {
    brand: string,
    model: string,
    startDate: string,
    endDate: string,
    status: rentStatus,
    startLocation: string // location in which the car was rented
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
        try {
            const query = new URLSearchParams({
                Status: filters.selectedRentStatus,
                'Car.Location': filters.selectedLocation,
            }).toString();

            const response = await fetch(`/api/rentals?${query}`);
            if (!response.ok) {
                throw new Error('Failed to fetch rentals');
            }

            const data = await response.json();
            console.log('Rentals:', data);
        } catch (error) {
            console.error('Error fetching rentals:', error);
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
