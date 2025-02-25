import React, {createContext, useCallback, useContext, useState} from 'react';

export enum RentStatus {
    Rented = "Rented",
    Returned = "Returned"
}

export interface Rent {
    rentId: number,
    brand: string,
    model: string,
    startDate: string,
    endDate: string,
    status: RentStatus
}


interface RentsContextProps {
    rents: Rent[];
    setRents: React.Dispatch<React.SetStateAction<Rent[]>>;
    fetchRents: () => Promise<void>;
    returnCar: (rentId: number) => Promise<Response>;
}

const RentsContext = createContext<RentsContextProps | undefined>(undefined);

export const RentsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [rents, setRents] = useState<Rent[]>([]);

    // fetch offers based on filters
    const fetchRents = useCallback(async () => {
        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };
        
        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/get-user-rents`;

        try {
            const res = await fetch(apiUrl, {method: 'GET', headers: headers});
            const data : Rent[]= await res.json();
            console.log(data);
            setRents(data);
        }
        catch (error){
            console.log('Error fetching data', error);
        }
    }, []);
    
    const returnCar= useCallback(async (rentId: number): Promise<Response> => {
        console.log("returning car", rentId);
        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };
        
        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/return-car`;
        
        try {
            const res = await fetch(apiUrl, {
                method: 'POST',
                headers: headers,
                body: JSON.stringify(rentId)
            });

            if (!res.ok) {
                console.error(`Error: HTTP ${res.status} - ${res.statusText}`);
            }
            return res;
        }
        catch (error){
            console.error('Error returning car:', error);
            throw error;
        }
        
    }, []);

    return (
        <RentsContext.Provider value={{ rents, setRents, fetchRents, returnCar}}>
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
