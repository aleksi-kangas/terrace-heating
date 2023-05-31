interface CardProps {
  children: React.ReactNode;
  className?: string;
}

const Card = ({children, className}: CardProps): React.JSX.Element => {
  const styles = 'bg-white drop-shadow-lg rounded-xl';
  className = className ? styles.concat(' ', className) : styles;
  return <div className={className}>{children}</div>;
}

export default Card;
