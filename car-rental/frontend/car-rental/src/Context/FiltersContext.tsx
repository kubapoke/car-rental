import {createContext, ReactNode, useContext, useState} from "react";

export enum rentStatus {
    Active = "Active",
    ReadyToReturn = "ReadyToReturn",
    Returned = "Returned"
}

interface Filters {
    selectedRentStatus: rentStatus;
}

interface FiltersContextValue {
    filters: Filters;
    setFilters: React.Dispatch<React.SetStateAction<Filters>>;
}

const FiltersContext = createContext<FiltersContextValue | undefined>(undefined);

export const FiltersProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [filters, setFilters] = useState<Filters>({
        selectedRentStatus: rentStatus.Active,
    });

    return (
        <FiltersContext.Provider value={{ filters, setFilters }}>
            {children}
        </FiltersContext.Provider>
    );
};

export const useFilters = () => {
    const context = useContext(FiltersContext);
    if (!context) {
        throw new Error('useFilters must be used within a FiltersProvider');
    }
    return context;
};