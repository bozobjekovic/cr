import { ReactNode } from "react";
import Header from "./Header";

interface LayoutProps {
  children: ReactNode;
}

export const Layout = ({ children }: LayoutProps) => {
  return (
    <div className="App">
      <Header />
      
      <main className="main-content">
        <div className="content-container">
          {children}
        </div>
      </main>
    </div>
  );
}; 